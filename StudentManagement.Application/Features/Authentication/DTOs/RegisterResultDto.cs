using System;
using System.Collections.Generic;
using System.Text;

namespace StudentSkillManagement.Application.Features.Authentication.DTOs
{
    public sealed record RegisterResultDto(bool Succeeded, AuthResponseDto? Auth, IList<string> Errors);
}
