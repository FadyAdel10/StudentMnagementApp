using StudentSkillManagement.Application.Abstractions.Authentication;
using StudentSkillManagement.Application.Features.Authentication.DTOs;
using StudentSkillManagement.Domain.Entities.RefreshToken;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentSkillManagement.Application.Features.Authentication.Commands.Login
{
    public sealed class LoginCommandHandler
    {
        private readonly IIdentityService _identityService;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IRefreshTokenService _refreshTokenService;


        public LoginCommandHandler(IIdentityService identityService, IJwtTokenService jwtTokenService,
            IRefreshTokenRepository refreshTokenRepository, IRefreshTokenService refreshTokenService)
        {
            _identityService = identityService;
            _jwtTokenService = jwtTokenService;
            _refreshTokenRepository = refreshTokenRepository;
            _refreshTokenService = refreshTokenService;
        }

        async public Task<AuthResponseDto?> Handle(LoginCommand command)
        {
            var userId =  await _identityService.ValidateCredentialsAsync(command.Email, command.Password);

            if(userId == null)
            {
                return null;
            }
            IList<string> roles =  await _identityService.GetRolesAsync(userId.Value);

            var accessToken = _jwtTokenService.GenerateToken(userId.Value, command.Email, roles);

            var rawRefreshToken = _refreshTokenService.GenerateToken();

            var refreshToken = RefreshToken.Create(userId.Value, Guid.NewGuid(), _refreshTokenService.HashToken(rawRefreshToken), _refreshTokenService.GetExpirationDate());

            await _refreshTokenRepository.AddAsync(refreshToken);

            await _refreshTokenRepository.SaveChangesAsync();

            return new AuthResponseDto(userId.Value, command.Email, accessToken, rawRefreshToken);
        }
    } 
}
