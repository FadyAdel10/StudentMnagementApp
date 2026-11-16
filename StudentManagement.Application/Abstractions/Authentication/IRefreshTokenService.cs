using System;
using System.Collections.Generic;
using System.Text;

namespace StudentSkillManagement.Application.Abstractions.Authentication
{
    public interface IRefreshTokenService
    {
        string GenerateToken();
        string HashToken(string token);
        DateTime GetExpirationDate();
    }
}
