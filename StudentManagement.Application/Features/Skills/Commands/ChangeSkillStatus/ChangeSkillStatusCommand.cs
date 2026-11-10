using System;
using System.Collections.Generic;
using System.Text;

namespace StudentSkillManagement.Application.Features.Skills.Commands.ChangeSkillStatus
{
    public sealed record ChangeSkillStatusCommand(Guid Id, bool IsActive);
}
