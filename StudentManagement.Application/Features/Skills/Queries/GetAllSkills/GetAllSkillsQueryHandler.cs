using StudentSkillManagement.Application.Abstractions.Persistence;
using StudentSkillManagement.Application.Features.Skills.Queries.GetSkillById;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentSkillManagement.Application.Features.Skills.Queries.GetAllSkills
{
    public sealed class GetAllSkillsQueryHandler
    {
        private ISkillRepository _skillRepository;

        public GetAllSkillsQueryHandler(ISkillRepository skillRepository)
        {
            _skillRepository = skillRepository;
        }
        async public Task<List<SkillDetailsDto>> Handle(GetAllSkillsQuery request, CancellationToken cancellationToken= default)
        {
            var skills = await _skillRepository.GetAll(cancellationToken);
            List<SkillDetailsDto> skillDetailsDtos = new List<SkillDetailsDto>();

            foreach(var skill in skills)
            {
                skillDetailsDtos.Add(new SkillDetailsDto(skill.Id, skill.Name, skill.Description, skill.IsActive));
            }

            return skillDetailsDtos;
        }
    }
}
