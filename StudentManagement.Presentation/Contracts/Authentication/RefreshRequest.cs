using System.ComponentModel.DataAnnotations;

namespace StudentSkillManagement.Presentation.Contracts.Authentication
{
    public sealed record RefreshRequest(
        [Required]
        string RefreshToken);
}
