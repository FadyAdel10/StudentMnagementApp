using StudentSkillManagement.Application.Abstractions.Persistence;
using StudentSkillManagement.Domain.Entities.Skill;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentSkillManagement.Application.Features.Skills.Commands.CreateSkill
{
    public sealed class CreateSkillCommandHandler
    {
        private readonly ISkillRepository _skillRepository;

        public CreateSkillCommandHandler(ISkillRepository skillRepository)
        {
            _skillRepository = skillRepository;
        }

        public async Task<Guid> Handle(
            CreateSkillCommand command,
            CancellationToken cancellationToken = default)
        {
            if (await _skillRepository.ExistsByNameAsync(
                command.Name,
                cancellationToken))
            {
                throw new InvalidOperationException(
                    "A skill with this name already exists.");
            }

            var skill = Skill.Create(
                command.Name,
                command.Description);

            await _skillRepository.AddAsync(skill, cancellationToken);

            await _skillRepository.SaveChangesAsync(cancellationToken);

            return skill.Id;
        }
    }
}
