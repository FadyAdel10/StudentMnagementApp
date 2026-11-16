using StudentSkillManagement.Application.Abstractions.Authentication;
using StudentSkillManagement.Application.Features.Authentication.DTOs;
using StudentSkillManagement.Domain.Entities.RefreshToken;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;

namespace StudentSkillManagement.Application.Features.Authentication.Commands.Refresh
{
    public sealed class RefreshCommandHandler
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IIdentityService _identityService;

        public RefreshCommandHandler(IRefreshTokenRepository refreshTokenRepository,
            IRefreshTokenService refreshTokenService, IJwtTokenService jwtTokenService,
            IIdentityService identityService)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _refreshTokenService = refreshTokenService;
            _jwtTokenService = jwtTokenService;
            _identityService = identityService;
        }

        public async Task<AuthResponseDto?> Handle(RefreshCommand command, CancellationToken cancellationToken = default)
        {
            var hashToken = _refreshTokenService.HashToken(command.RefreshToken);
            var storedToken = await _refreshTokenRepository.GetByTokenHashAsync(hashToken);

            if(storedToken == null)
            {
                return null;
            }

            if(storedToken.RevokedAtUtc.HasValue)
            {
                var familyTokens = await _refreshTokenRepository.GetByFamilyIdAsync(storedToken.FamilyId, cancellationToken);
                foreach (var token in familyTokens)
                {
                    token.Revoke();
                }
                await _refreshTokenRepository.SaveChangesAsync(cancellationToken);

                return null;
            }
            if(storedToken.ExpiresAtUtc <= DateTime.UtcNow)
            {
                return null;
            }

            var userEmail = await _identityService.GetEmailAsync(storedToken.UserId);
            if (userEmail is null)
            {
                return null;
            }
                
            var userRoles = await _identityService.GetRolesAsync(storedToken.UserId);

            var newRefreshTokenRaw = _refreshTokenService.GenerateToken();
            var newRefreshTokenHash = _refreshTokenService.HashToken(newRefreshTokenRaw);
            var newRefreshToken  =  RefreshToken.Create(storedToken.UserId, storedToken.FamilyId, newRefreshTokenHash, _refreshTokenService.GetExpirationDate());

            

            storedToken.Revoke(newRefreshTokenHash);

            await _refreshTokenRepository.AddAsync(newRefreshToken, cancellationToken);
            var saved = await _refreshTokenRepository.TrySaveChangesAsync(cancellationToken);

            if (!saved)
                return null;

            var newAccessToken = _jwtTokenService.GenerateToken(storedToken.UserId, userEmail, userRoles);

            return new AuthResponseDto(
            storedToken.UserId,
            userEmail,
            newAccessToken,
            newRefreshTokenRaw);
        }
    }
}
