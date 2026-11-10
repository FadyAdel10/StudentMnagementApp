using Microsoft.EntityFrameworkCore;
using StudentSkillManagement.Application.Abstractions.Persistence;
using StudentSkillManagement.Domain.Entities.Skill;
using StudentSkillManagement.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentSkillManagement.Infrastructure.Repositories
{
    public sealed class SkillRepository : ISkillRepository
    {
        private readonly ApplicationDbContext _context;

        public SkillRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        async Task ISkillRepository.AddAsync(Skill skill, CancellationToken cancellationToken)
        {
            await _context.Skills.AddAsync(skill, cancellationToken);
        }

        Task<bool> ISkillRepository.Delete(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return await _context.Skills
                .AnyAsync(
                    skill => skill.Name == name,
                    cancellationToken);
        }
        public async Task<bool> ExistsByNameAsync(string name, Guid excludedSkillId, CancellationToken cancellationToken = default)
        {
            return await _context.Skills
                .AnyAsync(
                    skill => skill.Name == name &&
                             skill.Id != excludedSkillId,
                    cancellationToken);
        }

        async Task<List<Skill>> ISkillRepository.GetAll(CancellationToken cancellationToken)
        {
            return await _context.Skills.ToListAsync<Skill>(cancellationToken);
        }

        async Task<Skill?> ISkillRepository.GetById(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Skills.FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        }

        async Task ISkillRepository.SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync();
        }

        Task<bool> ISkillRepository.Update(Guid id, Skill skill, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
