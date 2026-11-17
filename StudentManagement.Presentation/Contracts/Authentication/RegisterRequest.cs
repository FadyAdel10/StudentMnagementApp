using System.ComponentModel.DataAnnotations;

namespace StudentSkillManagement.Presentation.Contracts.Authentication
{
    public sealed record RegisterRequest(
        [Required] [DataType(DataType.EmailAddress)]
        string Email,

        [Required] [DataType(DataType.Password)]
        string Password);
}
