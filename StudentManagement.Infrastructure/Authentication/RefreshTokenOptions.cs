using System;
using System.Collections.Generic;
using System.Text;

namespace StudentSkillManagement.Infrastructure.Authentication
{
    public sealed class RefreshTokenOptions
    {
        public const string SectionName = "RefreshToken";

        public int ExpirationInDays { get; set; }
    }
}
