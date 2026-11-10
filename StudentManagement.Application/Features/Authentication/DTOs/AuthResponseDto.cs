using System;
using System.Collections.Generic;
using System.Text;

namespace StudentSkillManagement.Application.Features.Authentication.DTOs
{
    public sealed record AuthResponseDto(Guid UserId, string Email, string Token);
}
