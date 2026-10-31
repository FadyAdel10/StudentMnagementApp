using StudentSkillManagement.Domain.Entities.Student;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentSkillManagement.Domain.Entities.StudentSkill
{
    public sealed class StudentSkill
    {
        public Guid StudentId { get; private set; }
        public Guid SkillId { get; private set; }
        public ProficiencyLevel ProficiencyLevel { get; private set; }
        public Student.Student Student { get; private set; }
        = null!;
        public Skill.Skill Skill { get; private set; } = null!;

        private StudentSkill()
        {
            // Reserved for ORM materialization.
        }

        private StudentSkill(
            Guid studentId,
            Guid skillId,
            ProficiencyLevel proficiencyLevel)
        {
            if(studentId == Guid.Empty)
            {
                throw new ArgumentException("Student ID is required.",nameof(studentId));
            }
            if(skillId == Guid.Empty)
            {
                throw new ArgumentException("Skill ID is required.",nameof(skillId));
            }
            ValidateProficiency(proficiencyLevel);

            StudentId = studentId;
            SkillId = skillId;
            ProficiencyLevel = proficiencyLevel;

        }

        private static void ValidateProficiency(
            ProficiencyLevel proficiency)
        {
            if (!Enum.IsDefined(proficiency))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(proficiency),
                    "Invalid proficiency level.");
            }
        }

        public static StudentSkill Create(
            Guid studentId,
            Guid skillId,
            ProficiencyLevel proficiency)
        {
            return new StudentSkill(studentId, skillId, proficiency);
        }

        public void UpdateStudentSkill(
            ProficiencyLevel proficiency)
        {
            ValidateProficiency(proficiency);

            ProficiencyLevel = proficiency;
        }

    }
}
