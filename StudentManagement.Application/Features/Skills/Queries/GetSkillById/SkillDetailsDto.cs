using System;
using System.Collections.Generic;
using System.Text;

namespace StudentSkillManagement.Application.Features.Skills.Queries.GetSkillById
{
    public sealed record SkillDetailsDto(Guid Id, string Name, string? Description, bool IsActive);
}
