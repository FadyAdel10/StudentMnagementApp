using StudentSkillManagement.Application.Abstractions.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentSkillManagement.Application.Features.Students.Commands.ChangeStudentStatus
{
    public sealed class ChangeStudentStatusCommandHandler
    {
        private IStudentRepository _studentRepository;

        public ChangeStudentStatusCommandHandler(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        async public Task Handle(ChangeStudentStatusCommand command, CancellationToken cancellationToken= default)
        {
            var student = await _studentRepository.GetById(command.Id, cancellationToken);
            if(student == null)
            {
                throw new KeyNotFoundException("Student not found.");
            }
            if (command.IsActive)
            {
                student.Activate();
            }
            else
            {
                student.Deactivate();
            }

            await _studentRepository.SaveChangesAsync();
        }
    }
}
