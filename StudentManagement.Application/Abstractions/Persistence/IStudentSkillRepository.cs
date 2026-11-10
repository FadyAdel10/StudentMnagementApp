using StudentSkillManagement.Domain.Entities.Skill;
using StudentSkillManagement.Domain.Entities.StudentSkill;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentSkillManagement.Application.Abstractions.Persistence
{
    public interface IStudentSkillRepository
    {
        Task AddAsync(StudentSkill studentSkill, CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(Guid studentId, Guid skillId, CancellationToken cancellationToken = default);
        Task<List<StudentSkill>> GetAllStudentSkills(Guid studentId, CancellationToken cancellationToken = default);
        Task<StudentSkill?> GetStudentSkill(Guid studentId, Guid skillId, CancellationToken cancellationToken = default);
        void Delete(StudentSkill studentSkill);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
