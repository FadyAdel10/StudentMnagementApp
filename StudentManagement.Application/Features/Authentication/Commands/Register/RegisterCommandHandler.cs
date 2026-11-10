using StudentSkillManagement.Application.Abstractions.Authentication;
using StudentSkillManagement.Application.Features.Authentication.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentSkillManagement.Application.Features.Authentication.Commands.Register
{
    public sealed class RegisterCommandHandler
    {
        private readonly IIdentityService _identityService;
        private readonly IJwtTokenService _jwtTokenService;

        public RegisterCommandHandler(IIdentityService identityService, IJwtTokenService jwtTokenService)
        {
            _identityService = identityService;
            _jwtTokenService = jwtTokenService;
        }

        async public Task<RegisterResultDto> Handle(RegisterCommand command)
        {
            var result =  await _identityService.RegisterAsync(command.Email, command.Password);

            // case error in registeration
            if(!result.Succeeded)
            {
                return new RegisterResultDto(false, null, result.Errors);
            }

            var userId = result.UserId!.Value;

            var roles =  await _identityService.GetRolesAsync(userId);

            var token =   _jwtTokenService.GenerateToken(userId, command.Email, roles);

            return new RegisterResultDto(true, new AuthResponseDto(userId, command.Email, token), Array.Empty<string>());

        }
    }
}
