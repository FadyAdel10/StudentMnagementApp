using System.ComponentModel.DataAnnotations;

namespace StudentSkillManagement.Presentation.Contracts.Authentication
{
    public sealed record LoginRequest(
        [Required] [DataType(DataType.EmailAddress)]
        string Email,

        [Required] [DataType(DataType.Password)]
        string Password);
}
