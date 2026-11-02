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

        public async Task<bool> Delete(Guid id, CancellationToken cancellationToken)
        {
            var studentToDelete = await _context.Students.FirstOrDefaultAsync(s => s.Id == id);
            if (studentToDelete == null)
            {
                return false;
            }
             _context.Students.Remove(studentToDelete);
            return true;
        }

        public async Task<List<Student>> GetAll(CancellationToken cancellationToken)
        {
            return await _context.Students.ToListAsync<Student>(cancellationToken);
        }

        public async Task<Student?> GetById(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Students.FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        }

        public async Task<bool> Update(Guid id, Student student, CancellationToken cancellationToken)
        {
            var studentToUpdate = await _context.Students.FirstOrDefaultAsync(s => s.Id == id);
            if(studentToUpdate == null)
            {
                return false;
            }
            studentToUpdate.UpdateProfile(student.FirstName, student.LastName, student.Email);
            return true;
        }
    }
}
