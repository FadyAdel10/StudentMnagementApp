using System;
using System.Collections.Generic;
using System.Text;

namespace StudentSkillManagement.Application.Features.Students.Commands.UpdateStudentProfile
{
    public sealed record UpdateStudentProfileCommand(Guid Id, string firstName, string lastName, string Email);
}
