using StudentSkillManagement.Domain.Entities.StudentSkill;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentSkillManagement.Application.Features.StudentSkills.DTOs
{
    public sealed record StudentSkillDetailsDto(Guid SkillId, string SkillName, int ProficiencyLevel,
        string ProficiencyLevelName);
}
