using System;
using System.Collections.Generic;
using System.Text;

namespace StudentSkillManagement.Application.Abstractions.Authentication
{
    public sealed record IdentityOperationResult(
        bool Succeeded,
        Guid? UserId,
        IList<string> Errors);

}
