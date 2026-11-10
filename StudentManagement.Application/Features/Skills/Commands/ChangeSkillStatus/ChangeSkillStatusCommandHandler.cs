using StudentSkillManagement.Application.Abstractions.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentSkillManagement.Application.Features.Skills.Commands.ChangeSkillStatus
{
    public sealed class ChangeSkillStatusCommandHandler
    {
        private ISkillRepository _skillRepository;

        public ChangeSkillStatusCommandHandler(ISkillRepository skillRepository)
        {
            _skillRepository = skillRepository;
        }

        async public Task Handle(ChangeSkillStatusCommand command, CancellationToken cancellationToken=default)
        {
            var skill = await _skillRepository.GetById(command.Id, cancellationToken);

            if(skill == null)
            {
                throw new KeyNotFoundException("Skill not found.");
            }

            if(command.IsActive)
            {
                skill.Activate();
            }
            else
            {
                skill.Deactivate();
            }

            await _skillRepository.SaveChangesAsync(cancellationToken);
        }
    }
}
