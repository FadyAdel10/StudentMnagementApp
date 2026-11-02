using System;
using System.Collections.Generic;
using System.Text;

namespace StudentSkillManagement.Application.Features.Students.Commands.ChangeStudentStatus
{
    public sealed record ChangeStudentStatusCommand(Guid Id, bool IsActive);
}
