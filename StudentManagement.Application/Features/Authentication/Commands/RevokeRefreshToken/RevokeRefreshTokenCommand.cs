using System;
using System.Collections.Generic;
using System.Text;

namespace StudentSkillManagement.Application.Features.Authentication.Commands.RevokeRefreshToken
{
    public sealed record RevokeRefreshTokenCommand(string RefreshToken);
}
