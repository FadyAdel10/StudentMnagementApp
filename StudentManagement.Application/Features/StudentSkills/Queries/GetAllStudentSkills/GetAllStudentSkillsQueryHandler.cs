using StudentSkillManagement.Application.Abstractions.Persistence;
using StudentSkillManagement.Application.Features.StudentSkills.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentSkillManagement.Application.Features.StudentSkills.Queries.GetAllStudentSkills
{
    public sealed class GetAllStudentSkillsQueryHandler
    {
        private IStudentRepository _studentRepository;
        private IStudentSkillRepository _studentSkillRepository;

        public GetAllStudentSkillsQueryHandler(IStudentRepository studentRepository, IStudentSkillRepository studentSkillRepository)
        {
            _studentRepository = studentRepository;
            _studentSkillRepository = studentSkillRepository;
        }

        async public Task<List<StudentSkillDetailsDto>> Handle(GetAllStudentSkillsQuery query, CancellationToken cancellationToken = default)
        {
            var studentSkills = await _studentSkillRepository.GetAllStudentSkills(query.StudentId, cancellationToken);
            var studentSkillDetails = new List<StudentSkillDetailsDto>();

            foreach(var studentSkill in studentSkills)
            {
                studentSkillDetails.Add(new StudentSkillDetailsDto(studentSkill.SkillId, studentSkill.Skill.Name,
                    (int)studentSkill.ProficiencyLevel, studentSkill.ProficiencyLevel.ToString()));
            }
           
            return studentSkillDetails;
        }
    }
}
