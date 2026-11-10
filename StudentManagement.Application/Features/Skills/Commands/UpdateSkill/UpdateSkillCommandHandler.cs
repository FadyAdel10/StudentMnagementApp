using StudentSkillManagement.Application.Abstractions.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentSkillManagement.Application.Features.Skills.Commands.UpdateSkill
{
    public sealed class UpdateSkillCommandHandler
    {
        private ISkillRepository _skillRepository;

        public UpdateSkillCommandHandler(ISkillRepository skillRepository)
        {
            _skillRepository = skillRepository;
        }

        async public Task Handle(UpdateSkillCommand request, CancellationToken cancellationToken=default)
        {
            var skill = await _skillRepository.GetById(request.Id);

            if(skill == null)
            {
                throw new KeyNotFoundException("Skill not found.");
            }
            if (await _skillRepository.ExistsByNameAsync(request.Name,request.Id,cancellationToken))
            {
                throw new InvalidOperationException(
                    "A skill with this name already exists.");
            }


            skill.UpdateSkill(request.Name, request.Description);
            await _skillRepository.SaveChangesAsync(cancellationToken);
        }
    }
}
