using StudentSkillManagement.Domain.Entities.Student;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentSkillManagement.Application.Abstractions.Persistence
{
    public interface IStudentRepository
    {
        Task AddAsync(Student student, CancellationToken cancellationToken= default);
        Task<Student?> GetById(Guid id, CancellationToken cancellationToken= default);
        Task<List<Student>> GetAll(CancellationToken cancellationToken= default);
        Task<bool> Update(Guid id, Student student, CancellationToken cancellationToken = default);
        Task<bool> Delete(Guid id, CancellationToken cancellationToken = default);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
