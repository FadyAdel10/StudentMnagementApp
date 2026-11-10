using Microsoft.AspNetCore.Identity;
using StudentSkillManagement.Application.Abstractions.Authentication;
using StudentSkillManagement.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentSkillManagement.Infrastructure.Authentication
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public IdentityService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        

        public async Task<IdentityOperationResult> RegisterAsync(
        string email,
        string password)
        {
            var user = new ApplicationUser
            {
                UserName = email,
                Email = email
            };

            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                var errors = result.Errors
                    .Select(error => error.Description)
                    .ToList();

                return new IdentityOperationResult(
                    false,
                    null,
                    errors);
            }

            return new IdentityOperationResult(
                true,
                user.Id,
                Array.Empty<string>());
        }

        async public Task<IList<string>> GetRolesAsync(Guid UserId)
        {
            var applicationUser = await _userManager.FindByIdAsync(UserId.ToString());

            if(applicationUser == null)
            {
                return Array.Empty<string>();
            }
            var roles = await _userManager.GetRolesAsync(applicationUser);
            return roles;
        }
    }
}
