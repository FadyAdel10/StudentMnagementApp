using System;
using System.Collections.Generic;
using System.Text;

namespace StudentSkillManagement.Application.Features.Skills.Commands.CreateSkill
{
    public sealed record CreateSkillCommand(
    string Name,
    string? Description);
}
