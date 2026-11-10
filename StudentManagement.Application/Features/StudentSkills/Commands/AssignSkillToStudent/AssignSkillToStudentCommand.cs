using StudentSkillManagement.Domain.Entities.StudentSkill;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentSkillManagement.Application.Features.StudentSkills.Commands.AssignSkillToStudent
{
    public sealed record AssignSkillToStudentCommand(Guid StudentId, Guid SkillId, ProficiencyLevel Proficiency);
}
