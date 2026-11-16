using Microsoft.Extensions.Options;
using StudentSkillManagement.Application.Abstractions.Authentication;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace StudentSkillManagement.Infrastructure.Authentication
{
    public sealed class RefreshTokenService : IRefreshTokenService
    {
        private readonly RefreshTokenOptions _options;

        public RefreshTokenService(IOptions<RefreshTokenOptions> options)
        {
            _options = options.Value;
        }
        public string GenerateToken()
        {
            var randomBytes = RandomNumberGenerator.GetBytes(64);

            return Convert.ToBase64String(randomBytes).TrimEnd('=').Replace('+', '-').Replace('/', '_');
        }

        public string HashToken(string token)
        {
            var tokenBytes = Encoding.UTF8.GetBytes(token);

            var hashBytes = SHA256.HashData(tokenBytes);

            return Convert.ToHexString(hashBytes);
        }
        public DateTime GetExpirationDate()
        {
            return DateTime.UtcNow.AddDays(_options.ExpirationInDays);
        }
    }
}
