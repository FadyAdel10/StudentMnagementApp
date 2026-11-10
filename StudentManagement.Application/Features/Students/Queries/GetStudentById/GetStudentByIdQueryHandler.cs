using StudentSkillManagement.Application.Abstractions.Persistence;
using StudentSkillManagement.Application.Features.StudentSkills.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentSkillManagement.Application.Features.Students.Queries.GetStudentById
{
    // continue implementing the GetStudentByIdQueryHandler use case
    public sealed class GetStudentByIdQueryHandler
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IStudentSkillRepository _studentSkillRepository;
        public GetStudentByIdQueryHandler(IStudentRepository studentRepository,
            IStudentSkillRepository studentSkillRepository)
        {
            _studentRepository = studentRepository;
            _studentSkillRepository = studentSkillRepository;
        }

        public async Task<StudentDetailsDto?> Handle(GetStudentByIdQuery request, CancellationToken cancellationToken= default)
        {
            // get student by id from the repository
            var student = await _studentRepository.GetById(request.Id, cancellationToken);
            if(student == null)
            {
                return null;
            }
            var studentSkills = await _studentSkillRepository.GetAllStudentSkills(request.Id, cancellationToken);

            var skills = studentSkills.Select(ss => new StudentSkillDetailsDto(
                ss.SkillId,
                ss.Skill.Name,
                (int)ss.ProficiencyLevel,
                ss.ProficiencyLevel.ToString()
            )).ToList();
            
            var studentDetailsDto = new StudentDetailsDto(
                student.Id,
                student.FirstName,
                student.LastName,
                student.Email,
                student.IsActive,
                skills
            );
            return studentDetailsDto;
        }
    }
}
