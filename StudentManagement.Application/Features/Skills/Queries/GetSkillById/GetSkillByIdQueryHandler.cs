using StudentSkillManagement.Application.Abstractions.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentSkillManagement.Application.Features.Skills.Queries.GetSkillById
{
    public sealed class GetSkillByIdQueryHandler
    {
        private readonly ISkillRepository _skillRepository;

        public GetSkillByIdQueryHandler(
            ISkillRepository skillRepository)
        {
            _skillRepository = skillRepository;
        }

        public async Task<SkillDetailsDto?> Handle(
            GetSkillByIdQuery query,
            CancellationToken cancellationToken = default)
        {
            var skill = await _skillRepository.GetById(
                query.Id,
                cancellationToken);

            if (skill == null)
                return null;

            return new SkillDetailsDto(
                skill.Id,
                skill.Name,
                skill.Description,
                skill.IsActive);
        }
    }
}
