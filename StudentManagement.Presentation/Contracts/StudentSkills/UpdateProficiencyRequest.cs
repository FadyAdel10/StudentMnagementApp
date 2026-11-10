using StudentSkillManagement.Domain.Entities.StudentSkill;

namespace StudentSkillManagement.Presentation.Contracts.StudentSkills
{
    public sealed record UpdateProficiencyRequest(ProficiencyLevel Proficiency);
}
