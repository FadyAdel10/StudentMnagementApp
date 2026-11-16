using StudentSkillManagement.Application.Abstractions.Authentication;
using StudentSkillManagement.Application.Features.Authentication.DTOs;
using StudentSkillManagement.Domain.Entities.RefreshToken;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentSkillManagement.Application.Features.Authentication.Commands.Register
{
    public sealed class RegisterCommandHandler
    {
        private readonly IIdentityService _identityService;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IRefreshTokenService _refreshTokenService;

        public RegisterCommandHandler(IIdentityService identityService, IJwtTokenService jwtTokenService,
            IRefreshTokenRepository refreshTokenRepository, IRefreshTokenService refreshTokenService)
        {
            _identityService = identityService;
            _jwtTokenService = jwtTokenService;
            _refreshTokenRepository = refreshTokenRepository;
            _refreshTokenService = refreshTokenService;
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

            var accessToken =   _jwtTokenService.GenerateToken(userId, command.Email, roles);

            var rawRefreshToken = _refreshTokenService.GenerateToken();

            var refreshToken =  RefreshToken.Create(userId, Guid.NewGuid(), _refreshTokenService.HashToken(rawRefreshToken), _refreshTokenService.GetExpirationDate());

            await _refreshTokenRepository.AddAsync(refreshToken);

            await _refreshTokenRepository.SaveChangesAsync();

            return new RegisterResultDto(true, new AuthResponseDto(userId, command.Email, accessToken, rawRefreshToken), Array.Empty<string>());

        }
    }
}
