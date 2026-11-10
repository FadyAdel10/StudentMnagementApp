using StudentSkillManagement.Application.Abstractions.Persistence;
using StudentSkillManagement.Domain.Entities.StudentSkill;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentSkillManagement.Application.Features.StudentSkills.Commands.UpdateProficiencyLevel
{
    public sealed class UpdateProficiencyLevelCommandHandler
    {
        private IStudentRepository _studentRepository;
        private IStudentSkillRepository _studentSkillRepository;

        public UpdateProficiencyLevelCommandHandler(IStudentSkillRepository studentSkillRepository,
            IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
            _studentSkillRepository = studentSkillRepository;
        }

        public async Task Handle(UpdateProficiencyLevelCommand command,
        CancellationToken cancellationToken = default)
        {
            var student = await _studentRepository.GetById(
                command.StudentId,
                cancellationToken);

            if (student is null)
                throw new KeyNotFoundException("Student not found.");

            var studentSkill =
                await _studentSkillRepository.GetStudentSkill(
                    command.StudentId,
                    command.SkillId,
                    cancellationToken);

            if (studentSkill is null)
                throw new KeyNotFoundException(
                    "This skill is not assigned to the student.");

            studentSkill.UpdateProficiency(command.Proficiency);

            await _studentSkillRepository.SaveChangesAsync(cancellationToken);
        }
    }
}
