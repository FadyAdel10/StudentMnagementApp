using System.ComponentModel.DataAnnotations;

namespace StudentSkillManagement.Presentation.Contracts.Skills
{
    public sealed record UpdateSkillRequest(
        [Required]
        [MaxLength(100)]
        string Name,

        [MaxLength(500)]
        string? Description);
}
