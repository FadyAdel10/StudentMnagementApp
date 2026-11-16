using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using StudentSkillManagement.Application.Features.Authentication.Commands.Login;
using StudentSkillManagement.Application.Features.Authentication.Commands.Refresh;
using StudentSkillManagement.Application.Features.Authentication.Commands.Register;
using StudentSkillManagement.Application.Features.Authentication.Commands.RevokeRefreshToken;
using StudentSkillManagement.Application.Features.Skills.Commands.CreateSkill;
using StudentSkillManagement.Presentation.Contracts.Authentication;
using StudentSkillManagement.Presentation.Contracts.Skills;

namespace StudentSkillManagement.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly RegisterCommandHandler _registerCommandHandler;
        private readonly LoginCommandHandler _loginCommandHandler;
        private readonly RefreshCommandHandler _refreshCommandHandler;
        private readonly RevokeRefreshTokenCommandHandler _revokeRefreshTokenCommandHandler;

        public AuthController(RegisterCommandHandler registerCommandHandler, LoginCommandHandler loginCommandHandler,
            RefreshCommandHandler refreshCommandHandler, RevokeRefreshTokenCommandHandler revokeRefreshTokenCommandHandler)
        {
            _registerCommandHandler = registerCommandHandler;
            _loginCommandHandler = loginCommandHandler;
            _refreshCommandHandler = refreshCommandHandler;
            _revokeRefreshTokenCommandHandler = revokeRefreshTokenCommandHandler;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Contracts.Authentication.RegisterRequest request)
        {
            var command = new RegisterCommand(request.Email, request.Password);

            var result = await _registerCommandHandler.Handle(command);

            if (!result.Succeeded)
                return BadRequest(new { result.Errors });

            return Ok(result.Auth);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Contracts.Authentication.LoginRequest request)
        {
            var command = new LoginCommand(request.Email, request.Password);

            var result = await _loginCommandHandler.Handle(command);

            if (result == null)
                return Unauthorized(new
                {
                    message = "Invalid email or password."
                });

            return Ok(result);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] Contracts.Authentication.RefreshRequest request, CancellationToken cancellationToken = default)
        {
            var command = new RefreshCommand(request.RefreshToken);

            var result = await _refreshCommandHandler.Handle(command, cancellationToken);

            if (result == null)
                return Unauthorized(new
                {
                    message = "Invalid or expired refresh token."
                });

            return Ok(result);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] RevokeRefreshTokenRequest request, CancellationToken cancellationToken = default)
        {
            var command = new RevokeRefreshTokenCommand(request.RefreshToken);

            var result = await _revokeRefreshTokenCommandHandler.Handle(command, cancellationToken);

            if (!result)
                return Unauthorized(new
                {
                    message = "Error in revoke refresh token"
                });

            return NoContent();
        }
    }
}
