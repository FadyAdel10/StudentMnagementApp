using StudentSkillManagement.Domain.Entities.Skill;
using StudentSkillManagement.Domain.Entities.Student;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentSkillManagement.Application.Abstractions.Persistence
{
    public interface ISkillRepository
    {
        Task AddAsync(Skill skill, CancellationToken cancellationToken = default);
        Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);
        Task<bool> ExistsByNameAsync(string name, Guid excludedSkillId, CancellationToken cancellationToken = default);
        Task<Skill?> GetById(Guid id, CancellationToken cancellationToken = default);
        Task<List<Skill>> GetAll(CancellationToken cancellationToken = default);
        Task<bool> Update(Guid id, Skill skill, CancellationToken cancellationToken = default);
        Task<bool> Delete(Guid id, CancellationToken cancellationToken = default);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
