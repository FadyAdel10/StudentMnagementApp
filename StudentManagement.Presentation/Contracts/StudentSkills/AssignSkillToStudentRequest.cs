using StudentSkillManagement.Domain.Entities.StudentSkill;

namespace StudentSkillManagement.Presentation.Contracts.StudentSkills
{
    public sealed record AssignSkillToStudentRequest(Guid SkillId, ProficiencyLevel Proficiency);
}
