using StudentSkillManagement.Application.Abstractions.Authentication;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace StudentSkillManagement.Application.Features.Authentication.Commands.RevokeRefreshToken
{
    public sealed class RevokeRefreshTokenCommandHandler
    {
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public RevokeRefreshTokenCommandHandler(IRefreshTokenService refreshTokenService, IRefreshTokenRepository refreshTokenRepository)
        {
            _refreshTokenService = refreshTokenService;
            _refreshTokenRepository = refreshTokenRepository;
        }


        async public Task<bool> Handle(RevokeRefreshTokenCommand command, CancellationToken cancellationToken = default)
        {
            var tokenHash = _refreshTokenService.HashToken(command.RefreshToken);
            var storedToken = await _refreshTokenRepository.GetByTokenHashAsync(tokenHash);

            if(storedToken ==  null || !storedToken.IsActive())
            {
                return false;
            }
            var familyTokens = await _refreshTokenRepository.GetByFamilyIdAsync(storedToken.FamilyId, cancellationToken);

            foreach(var token in familyTokens)
            {
                token.Revoke();
            }

            return await _refreshTokenRepository.TrySaveChangesAsync(cancellationToken);
        }
    }
}
