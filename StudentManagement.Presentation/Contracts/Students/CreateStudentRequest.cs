using System.ComponentModel.DataAnnotations;

namespace StudentSkillManagement.Presentation.Contracts.Students
{
    public sealed record CreateStudentRequest
    (
        [Required]
        [MaxLength(100)]
        string FirstName,

        [Required]
        [MaxLength(100)]
            string LastName,

        [Required]
        [EmailAddress]
            [MaxLength(255)]
            string Email
    );
}
