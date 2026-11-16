using System;
using System.Collections.Generic;
using System.Text;

namespace StudentSkillManagement.Application.Features.Authentication.Commands.Refresh
{
    public sealed record RefreshCommand(string RefreshToken);
}
