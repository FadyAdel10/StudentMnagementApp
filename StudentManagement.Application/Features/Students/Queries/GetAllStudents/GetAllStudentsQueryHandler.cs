using StudentSkillManagement.Application.Abstractions.Persistence;
using StudentSkillManagement.Application.Features.Students.Queries.GetStudentById;
using StudentSkillManagement.Application.Features.StudentSkills.DTOs;
using StudentSkillManagement.Domain.Entities.StudentSkill;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentSkillManagement.Application.Features.Students.Queries.GetAllStudents
{
    public sealed class GetAllStudentsQueryHandler
    {
        private IStudentRepository _studentRepository;
        private readonly IStudentSkillRepository _studentSkillRepository;

        public GetAllStudentsQueryHandler(IStudentRepository studentRepository,
            IStudentSkillRepository studentSkillRepository)
        {
            _studentRepository = studentRepository;
            _studentSkillRepository = studentSkillRepository;
        }

        public async Task<List<StudentDetailsDto>> Handle(GetAllStudentsQuery request,CancellationToken cancellationToken = default)
        {
            var students = await _studentRepository.GetAll(request.SearchTerm, request.IsActive);
            List<StudentDetailsDto> studentsDto = new List<StudentDetailsDto>();
            foreach (var student in students)
            {
                var studentSkills = await _studentSkillRepository.GetAllStudentSkills(student.Id, cancellationToken);
                var skills = studentSkills.Select(ss => new StudentSkillDetailsDto(
                    ss.SkillId,
                    ss.Skill.Name,
                    (int)ss.ProficiencyLevel,
                    ss.ProficiencyLevel.ToString()
                )).ToList();
                studentsDto.Add(new StudentDetailsDto(student.Id,student.FirstName, student.LastName, student.Email, student.IsActive,skills));
            }

            return studentsDto;
        }
    }
}
