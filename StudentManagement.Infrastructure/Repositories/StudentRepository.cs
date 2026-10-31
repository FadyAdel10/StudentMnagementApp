using Microsoft.EntityFrameworkCore;
using StudentSkillManagement.Application.Abstractions.Persistence;
using StudentSkillManagement.Domain.Entities.Student;
using StudentSkillManagement.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentSkillManagement.Infrastructure.Repositories
{
    public sealed class StudentRepository : IStudentRepository
    {
        private readonly ApplicationDbContext _context;

        public StudentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(
            Student student,
            CancellationToken cancellationToken = default)
        {
            await _context.Students.AddAsync(
                student,
                cancellationToken);
        }

        public async Task SaveChangesAsync(
            CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        public Task<bool> Delete(Guid id, CancellationToken cancellationToken)
        {
            var studentToDelete = _context.Students.FirstOrDefault(s => s.Id == id);
            if (studentToDelete == null)
            {
                return Task.FromResult(false);
            }
             _context.Students.Remove(studentToDelete);
            return Task.FromResult(true);
        }

        async Task<List<Student>> IStudentRepository.GetAll(CancellationToken cancellationToken)
        {
            //return _context.Students.ToListAsync<Student>(cancellationToken);
            throw new NotImplementedException();
        }

        Task<Student?> IStudentRepository.GetById(Guid id, CancellationToken cancellationToken)
        {
            return _context.Students.FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        }

        Task<bool> IStudentRepository.Update(Guid id, Student student, CancellationToken cancellationToken)
        {
            var studentToUpdate = _context.Students.FirstOrDefault(s => s.Id == id);
            if(studentToUpdate == null)
            {
                return Task.FromResult(false);
            }
            studentToUpdate.UpdateProfile(student.FirstName, student.LastName, student.Email);
            return Task.FromResult(true);
        }
    }
}
