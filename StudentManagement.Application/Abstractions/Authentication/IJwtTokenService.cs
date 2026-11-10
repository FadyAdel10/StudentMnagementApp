using System;
using System.Collections.Generic;
using System.Text;

namespace StudentSkillManagement.Application.Abstractions.Authentication
{
    public interface IJwtTokenService
    {
        string GenerateToken(Guid userId, string email, IEnumerable<string> roles);
    }
}
