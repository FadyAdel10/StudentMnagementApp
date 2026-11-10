using StudentSkillManagement.Domain.Entities.StudentSkill;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentSkillManagement.Application.Features.StudentSkills.Commands.UpdateProficiencyLevel
{
    public sealed record UpdateProficiencyLevelCommand(Guid StudentId, Guid SkillId, ProficiencyLevel Proficiency);
}
