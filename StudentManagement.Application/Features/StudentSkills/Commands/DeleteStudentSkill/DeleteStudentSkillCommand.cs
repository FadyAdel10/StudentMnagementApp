using System;
using System.Collections.Generic;
using System.Text;

namespace StudentSkillManagement.Application.Features.StudentSkills.Commands.DeleteStudentSkill
{
    public sealed record DeleteStudentSkillCommand(Guid StudentId, Guid SkillId, CancellationToken cancellationToken = default);
}
