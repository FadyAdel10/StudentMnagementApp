using System;
using System.Collections.Generic;
using System.Text;

namespace StudentSkillManagement.Application.Abstractions.Authentication
{
    public interface IIdentityService
    {
        Task<IdentityOperationResult> RegisterAsync(string Email, string Password);
        Task<IList<string>> GetRolesAsync(Guid UserId);
    }
}
