using StudentSkillManagement.Application.Abstractions.Persistence;
using StudentSkillManagement.Domain.Entities.StudentSkill;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace StudentSkillManagement.Application.Features.StudentSkills.Commands.AssignSkillToStudent
{
    public sealed class AssignSkillToStudentCommandHandler
    {
        private IStudentRepository _studentRepository;
        private ISkillRepository _skillRepository;
        private IStudentSkillRepository _studentSkillRepository;
        
        public AssignSkillToStudentCommandHandler(IStudentRepository studentRepository,
            ISkillRepository skillRepository,IStudentSkillRepository studentSkillRepository)
        {
            _studentRepository = studentRepository;
            _skillRepository = skillRepository;
            _studentSkillRepository = studentSkillRepository;
        }

        async public Task Handle(AssignSkillToStudentCommand command, CancellationToken cancellationToken = default)
        {
            var student = await _studentRepository.GetById(command.StudentId, cancellationToken);
            if (student == null)
            {
                throw new KeyNotFoundException("Student not found.");
            }
            if (!student.IsActive)
            {
                throw new InvalidOperationException("Cannot assign skills to an inactive student.");
            }

            var skill = await _skillRepository.GetById(command.SkillId, cancellationToken);
            if(skill == null)
            {
                throw new KeyNotFoundException("Skill not found.");
            }
            
            if(!skill.IsActive)
            {
                throw new InvalidOperationException("Cannot assign students to an inactive student.");
            }
            if (await _studentSkillRepository.ExistsAsync(command.StudentId, command.SkillId, cancellationToken))
            {
                throw new InvalidOperationException("This skill is already assigned to the student.");
            }

            var studentSkill = StudentSkill.Create(command.StudentId, command.SkillId, command.Proficiency);
            await _studentSkillRepository.AddAsync(studentSkill, cancellationToken);
            await _studentSkillRepository.SaveChangesAsync(cancellationToken);
        }
    }
}
