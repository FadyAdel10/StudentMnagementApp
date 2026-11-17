using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using StudentSkillManagement.Application.Features.Authentication.Commands.Login;
using StudentSkillManagement.Application.Features.Authentication.Commands.Refresh;
using StudentSkillManagement.Application.Features.Authentication.Commands.Register;
using StudentSkillManagement.Application.Features.Authentication.Commands.RevokeRefreshToken;
using StudentSkillManagement.Application.Features.Authentication.DTOs;
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

            var response = CreateAuthenticationResponse(result.Auth);


            return Ok(response);
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

            var response = CreateAuthenticationResponse(result);


            return Ok(response);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh( CancellationToken cancellationToken = default)
        {
            var refreshToken = Request.Cookies[RefreshTokenCookieSettings.CookieName];

            if (string.IsNullOrWhiteSpace(refreshToken))
                return Unauthorized();

            var command = new RefreshCommand(refreshToken);

            var result = await _refreshCommandHandler.Handle(command, cancellationToken);

            if (result == null)
                return Unauthorized(new
                {
                    message = "Invalid or expired refresh token."
                });

            var response = CreateAuthenticationResponse(result);


            return Ok(response);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout( CancellationToken cancellationToken = default)
        {
            var refreshToken = Request.Cookies[RefreshTokenCookieSettings.CookieName];

            if (string.IsNullOrWhiteSpace(refreshToken))
                return Unauthorized();

            var command = new RevokeRefreshTokenCommand(refreshToken);

            var result = await _revokeRefreshTokenCommandHandler.Handle(command, cancellationToken);

            if (!result)
                return Unauthorized(new
                {
                    message = "Error in revoke refresh token"
                });
            Response.Cookies.Delete(RefreshTokenCookieSettings.CookieName);

            return NoContent();
        }

        private AuthenticationResponse CreateAuthenticationResponse(
            AuthResponseDto auth)
            {
                Response.Cookies.Append(
                    RefreshTokenCookieSettings.CookieName,
                    auth.RefreshToken,
                    RefreshTokenCookieSettings.Create(auth.RefreshTokenExpiresAtUtc));

                return new AuthenticationResponse(
                    auth.UserId,
                    auth.Email,
                    auth.AccessToken
                );
            }
    }
}
