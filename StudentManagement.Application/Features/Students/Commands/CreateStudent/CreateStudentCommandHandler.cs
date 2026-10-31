using StudentSkillManagement.Application.Abstractions.Persistence;
using StudentSkillManagement.Domain.Entities.Student;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentSkillManagement.Application.Features.Students.Commands.CreateStudent
{
    public sealed class CreateStudentCommandHandler
    {
        private IStudentRepository _studentRepository;

        public CreateStudentCommandHandler(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<Guid> Handle(CreateStudentCommand command, CancellationToken cancellationToken= default)
        {
            // 1- create student
            var student = Student.Create(command.FirstName, command.LastName, command.Email);

            // 2- add sudent 
            await _studentRepository.AddAsync(student, cancellationToken);

            // 2- save sudent 
            await _studentRepository.SaveChangesAsync(cancellationToken);

            return student.Id;
        }
    }
}
