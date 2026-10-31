using System;
using System.Collections.Generic;
using System.Text;

namespace StudentSkillManagement.Application.Features.Students
{
    public sealed record CreateStudentCommand
    (
        string FirstName,
        string LastName,
        string Email
    );
}
