
using StudentSkillManagement.Domain.Entities.RefreshToken;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentSkillManagement.Application.Abstractions.Authentication
{
    public interface IRefreshTokenRepository
    {
        Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default);
        Task<RefreshToken?> GetByTokenHashAsync(string tokenHash, CancellationToken cancellationToken = default);
        Task<IList<RefreshToken>> GetByFamilyIdAsync(Guid familyId, CancellationToken cancellationToken = default);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
        Task<bool> TrySaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
