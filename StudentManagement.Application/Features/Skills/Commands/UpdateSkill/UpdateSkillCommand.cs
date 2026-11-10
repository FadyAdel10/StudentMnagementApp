using System;
using System.Collections.Generic;
using System.Text;

namespace StudentSkillManagement.Application.Features.Skills.Commands.UpdateSkill
{
    public sealed record UpdateSkillCommand(Guid Id, string Name, string? Description);
}
