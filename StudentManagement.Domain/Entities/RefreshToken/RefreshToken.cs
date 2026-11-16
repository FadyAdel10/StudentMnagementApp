using System;
using System.Collections.Generic;
using System.Text;

namespace StudentSkillManagement.Domain.Entities.RefreshToken
{
    

    public sealed class RefreshToken
    {
        public Guid Id { get; private set; }

        public Guid UserId { get; private set; }

        public Guid FamilyId { get; private set; }

        public string TokenHash { get; private set; } = string.Empty;

        public DateTime CreatedAtUtc { get; private set; }

        public DateTime ExpiresAtUtc { get; private set; }

        public DateTime? RevokedAtUtc { get; private set; }

        public string? ReplacedByTokenHash { get; private set; }
        public byte[] RowVersion { get; private set; } = [];

        private RefreshToken()
        {

        }

        public static RefreshToken Create(
            Guid userId,
            Guid familyId,
            string tokenHash,
            DateTime expiresAtUtc)
        {
            return new RefreshToken
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                FamilyId = familyId,
                TokenHash = tokenHash,
                CreatedAtUtc = DateTime.UtcNow,
                ExpiresAtUtc = expiresAtUtc
            };
        }

        public bool IsActive()
        {
            return RevokedAtUtc == null && ExpiresAtUtc > DateTime.UtcNow;
        }

        public void Revoke(string? replacedByTokenHash = null)
        {
            if (RevokedAtUtc.HasValue)
                return;

            RevokedAtUtc = DateTime.UtcNow;
            ReplacedByTokenHash = replacedByTokenHash;
        }
    }
}
