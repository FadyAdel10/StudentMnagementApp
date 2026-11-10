using System.ComponentModel.DataAnnotations;

namespace StudentSkillManagement.Presentation.Contracts.Skills
{
    public sealed record CreateSkillRequest(
    [Required]
    [MaxLength(100)]
    string Name,

    [MaxLength(500)]
    string? Description
);
}
