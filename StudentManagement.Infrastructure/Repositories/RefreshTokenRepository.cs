using Microsoft.EntityFrameworkCore;
using StudentSkillManagement.Application.Abstractions.Authentication;
using StudentSkillManagement.Domain.Entities.RefreshToken;
using StudentSkillManagement.Infrastructure.Authentication;
using StudentSkillManagement.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentSkillManagement.Infrastructure.Repositories
{
    public sealed class RefreshTokenRepository : IRefreshTokenRepository
    {
        // continue implementation from here
        private readonly ApplicationDbContext _context;

        public RefreshTokenRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        async public Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default)
        {
            await _context.RefreshTokens.AddAsync(refreshToken, cancellationToken);
        }

        async public Task<RefreshToken?> GetByTokenHashAsync(string tokenHash, CancellationToken cancellationToken = default)
        {
            return await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.TokenHash == tokenHash, cancellationToken);
        }
        async public Task<IList<RefreshToken>> GetByFamilyIdAsync(Guid familyId,CancellationToken cancellationToken = default)
        {
            return await _context.RefreshTokens.Where(rt => rt.FamilyId == familyId).ToListAsync(cancellationToken);
        }
        async public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }


        public async Task<bool> TrySaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await _context.SaveChangesAsync(cancellationToken);
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }

        }
    }
}
