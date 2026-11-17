using System.ComponentModel.DataAnnotations;

namespace StudentSkillManagement.Presentation.Contracts.Authentication
{
    public sealed record RevokeRefreshTokenRequest(
        [Required]
        string RefreshToken);
}
