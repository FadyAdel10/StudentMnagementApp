using StudentSkillManagement.Application.Abstractions.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentSkillManagement.Application.Features.Students.Queries.GetStudentById
{
    // continue implementing the GetStudentByIdQueryHandler use case
    public sealed class GetStudentByIdQueryHandler
    {
        private readonly IStudentRepository _studentRepository;
        public GetStudentByIdQueryHandler(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<StudentDetailsDto?> Handle(GetStudentByIdQuery request, CancellationToken cancellationToken= default)
        {
            // get student by id from the repository
            var student = await _studentRepository.GetById(request.Id, cancellationToken);
            if(student == null)
            {
                return null;
            }
            var studentDetailsDto = new StudentDetailsDto(
                student.Id,
                student.FirstName,
                student.LastName,
                student.Email,
                student.IsActive
            );
            return studentDetailsDto;
        }
    }
}
