using StudentSkillManagement.Application.Abstractions.Persistence;
using StudentSkillManagement.Domain.Entities.Student;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentSkillManagement.Application.Features.Students.Commands.UpdateStudentProfile
{
    public sealed class UpdateStudentProfileCommandHandler
    {
        private IStudentRepository _studentRepository;

        public UpdateStudentProfileCommandHandler(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        async public Task Handle(UpdateStudentProfileCommand request, CancellationToken cancellationToken= default)
        {
            var student = await _studentRepository.GetById(request.Id);
            if(student == null)
            {
                throw new KeyNotFoundException("Student not found.");
            }

            student.UpdateProfile(request.firstName, request.lastName, request.Email);

            await _studentRepository.SaveChangesAsync(cancellationToken);
        }
    }
}
