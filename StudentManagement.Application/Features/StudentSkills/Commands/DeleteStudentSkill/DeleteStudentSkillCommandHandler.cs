using StudentSkillManagement.Application.Abstractions.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentSkillManagement.Application.Features.StudentSkills.Commands.DeleteStudentSkill
{
    public sealed class DeleteStudentSkillCommandHandler
    {
        private IStudentSkillRepository _studentSkillRepository;

        public DeleteStudentSkillCommandHandler(IStudentSkillRepository studentSkillRepository)
        {
            _studentSkillRepository = studentSkillRepository;
        }

        async public Task Handle(DeleteStudentSkillCommand command, CancellationToken cancellationToken = default)
        {
            var studentSkill = await _studentSkillRepository.GetStudentSkill(command.StudentId, command.SkillId, cancellationToken);

            if(studentSkill == null)
            {
                throw new InvalidOperationException("This skill is not assigned to that student.");
            }

            _studentSkillRepository.Delete(studentSkill);
            await _studentSkillRepository.SaveChangesAsync(cancellationToken);
        }
    }
}
