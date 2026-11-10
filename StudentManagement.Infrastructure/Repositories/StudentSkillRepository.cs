using Microsoft.EntityFrameworkCore;
using StudentSkillManagement.Application.Abstractions.Persistence;
using StudentSkillManagement.Domain.Entities.Skill;
using StudentSkillManagement.Domain.Entities.StudentSkill;
using StudentSkillManagement.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentSkillManagement.Infrastructure.Repositories
{
    public sealed class StudentSkillRepository : IStudentSkillRepository
    {
        private ApplicationDbContext _context;

        public StudentSkillRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        async public Task AddAsync(StudentSkill studentSkill, CancellationToken cancellationToken)
        {
            await _context.StudentSkills.AddAsync(studentSkill, cancellationToken);
        }
        async public Task<bool> ExistsAsync(Guid studentId, Guid skillId, CancellationToken cancellationToken)
        {
            return await _context.StudentSkills.AnyAsync(ss => ss.StudentId == studentId && ss.SkillId == skillId,
                cancellationToken);
        }
        async public Task<List<StudentSkill>> GetAllStudentSkills(Guid studentId, CancellationToken cancellationToken)
        {
            return await _context.StudentSkills.Include(ss => ss.Skill).Where(ss => ss.StudentId == studentId)
                .ToListAsync(cancellationToken);
        }
        async public Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        async public Task<StudentSkill?> GetStudentSkill(Guid studentId, Guid skillId, CancellationToken cancellationToken)
        {
            return await _context.StudentSkills.FirstOrDefaultAsync(ss => ss.StudentId == studentId && ss.SkillId == skillId);
        }

        public void Delete(StudentSkill studentSkill)
        {
            _context.StudentSkills.Remove(studentSkill);
        }
    }
}
