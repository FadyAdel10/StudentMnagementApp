using System;
using System.Collections.Generic;
using System.Text;

namespace StudentSkillManagement.Application.Features.Authentication.Commands.Register
{
    public sealed record RegisterCommand(
    string Email,
    string Password
    );
}
