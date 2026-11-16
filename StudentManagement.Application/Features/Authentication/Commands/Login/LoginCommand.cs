using System;
using System.Collections.Generic;
using System.Text;

namespace StudentSkillManagement.Application.Features.Authentication.Commands.Login
{
    public sealed record LoginCommand(string Email, string Password);
}
