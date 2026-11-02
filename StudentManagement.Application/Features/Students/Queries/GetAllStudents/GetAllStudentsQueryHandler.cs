using StudentSkillManagement.Application.Abstractions.Persistence;
using StudentSkillManagement.Application.Features.Students.Queries.GetStudentById;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentSkillManagement.Application.Features.Students.Queries.GetAllStudents
{
    public sealed class GetAllStudentsQueryHandler
    {
        private IStudentRepository _studentRepository;

        public GetAllStudentsQueryHandler(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<List<StudentDetailsDto>> Handle(GetAllStudentsQuery request)
        {
            var students = await _studentRepository.GetAll();
            List<StudentDetailsDto> studentsDto = new List<StudentDetailsDto>();
            foreach (var student in students)
            {
                studentsDto.Add(new StudentDetailsDto(student.Id,student.FirstName, student.LastName, student.Email, student.IsActive));
            }

            return studentsDto;
        }
    }
}
