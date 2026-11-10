using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentSkillManagement.Application.Features.Students;
using StudentSkillManagement.Application.Features.Students.Commands.ChangeStudentStatus;
using StudentSkillManagement.Application.Features.Students.Commands.CreateStudent;
using StudentSkillManagement.Application.Features.Students.Commands.UpdateStudentProfile;
using StudentSkillManagement.Application.Features.Students.Queries.GetAllStudents;
using StudentSkillManagement.Application.Features.Students.Queries.GetStudentById;
using StudentSkillManagement.Application.Features.StudentSkills.Commands.AssignSkillToStudent;
using StudentSkillManagement.Application.Features.StudentSkills.Commands.DeleteStudentSkill;
using StudentSkillManagement.Application.Features.StudentSkills.Commands.UpdateProficiencyLevel;
using StudentSkillManagement.Application.Features.StudentSkills.Queries.GetAllStudentSkills;
using StudentSkillManagement.Presentation.Contracts.Students;
using StudentSkillManagement.Presentation.Contracts.StudentSkills;
using System.Reflection.Metadata;

namespace StudentSkillManagement.Presentation.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly CreateStudentCommandHandler _createStudentCommandHandler;
        private readonly GetStudentByIdQueryHandler _getStudentByIdQueryHandler;
        private readonly GetAllStudentsQueryHandler _getAllStudentsQueryHandler;
        private readonly UpdateStudentProfileCommandHandler _updateStudentProfileCommandHandler;
        private readonly ChangeStudentStatusCommandHandler _changeStudentStatusCommandHandler;
        private readonly AssignSkillToStudentCommandHandler _assignSkillToStudentCommandHandler;
        private readonly GetAllStudentSkillsQueryHandler _getAllStudentSkillsQueryHandler;
        private readonly UpdateProficiencyLevelCommandHandler _updateProficiencyLevelCommandHandler;
        private readonly DeleteStudentSkillCommandHandler _deleteStudentSkillCommandHandler;

        public StudentsController(CreateStudentCommandHandler createStudentCommandHandler, GetStudentByIdQueryHandler getStudentByIdQueryHandler,
            GetAllStudentsQueryHandler getAllStudentsQueryHandler, UpdateStudentProfileCommandHandler updateStudentProfileCommandHandler,
            ChangeStudentStatusCommandHandler changeStudentStatusCommandHandler, AssignSkillToStudentCommandHandler assignSkillToStudentCommandHandler,
            GetAllStudentSkillsQueryHandler getAllStudentSkillsQueryHandler, UpdateProficiencyLevelCommandHandler updateProficiencyLevelCommandHandler,
            DeleteStudentSkillCommandHandler deleteStudentSkillCommandHandler)
        {
            _createStudentCommandHandler = createStudentCommandHandler;
            _getStudentByIdQueryHandler = getStudentByIdQueryHandler;
            _getAllStudentsQueryHandler = getAllStudentsQueryHandler;
            _updateStudentProfileCommandHandler = updateStudentProfileCommandHandler;
            _changeStudentStatusCommandHandler = changeStudentStatusCommandHandler;
            _assignSkillToStudentCommandHandler = assignSkillToStudentCommandHandler;
            _getAllStudentSkillsQueryHandler = getAllStudentSkillsQueryHandler;
            _updateProficiencyLevelCommandHandler = updateProficiencyLevelCommandHandler;
            _deleteStudentSkillCommandHandler = deleteStudentSkillCommandHandler;
        }
        [HttpPost]
        public async Task<IActionResult> Create(
        [FromBody] CreateStudentRequest request,
        CancellationToken cancellationToken)
        {
            var command = new CreateStudentCommand(
                request.FirstName,
                request.LastName,
                request.Email
            );

            var studentId = await _createStudentCommandHandler.Handle(command, cancellationToken);

            return CreatedAtAction(
                nameof(GetById),
                new { id = studentId },
                new { id = studentId });
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var query = new GetStudentByIdQuery(id);
            var student = await _getStudentByIdQueryHandler.Handle(query);

            if(student == null)
            {
                return NotFound();
            }
            return Ok(student);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery]string? searchTerm, [FromQuery]bool? isActive)
        {
            var query = new GetAllStudentsQuery(searchTerm, isActive);
            var students = await _getAllStudentsQueryHandler.Handle(query);
 
            return Ok(students);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody]UpdateStudentProfileRequest request, CancellationToken cancellationToken= default)
        {
            var command = new UpdateStudentProfileCommand(id, request.FirstName, request.LastName, request.Email);
            await _updateStudentProfileCommandHandler.Handle(command, cancellationToken);
            return NoContent();
        }

        [HttpPatch("{id:guid}/status")]
        public async Task<IActionResult> ChangeStatus(Guid id, [FromBody] ChangeStatusRequest request, CancellationToken cancellationToken= default)
        {
            var command = new ChangeStudentStatusCommand(id, request.IsActive);
            await _changeStudentStatusCommandHandler.Handle(command, cancellationToken);
            return NoContent();
        }

        [HttpPost("{studentId:guid}/skills")]
        public async Task<IActionResult> AssignSkill(Guid studentId, [FromBody] AssignSkillToStudentRequest request, CancellationToken cancellationToken = default)
        {
            var command = new AssignSkillToStudentCommand(studentId, request.SkillId,request.Proficiency);
            await _assignSkillToStudentCommandHandler.Handle(command, cancellationToken);
            return NoContent();
        }

        [HttpGet("{studentId:guid}/skills")]
        public async Task<IActionResult> GetSkills(Guid studentId,  CancellationToken cancellationToken = default)
        {
            var query = new GetAllStudentSkillsQuery(studentId);
            var studentSkillDetails = await _getAllStudentSkillsQueryHandler.Handle(query, cancellationToken);
            return Ok(studentSkillDetails);
        }

        [HttpPut("{studentId:guid}/skills/{skillId:guid}")]
        public async Task<IActionResult> GetSkills(Guid studentId, Guid skillId, [FromBody] UpdateProficiencyRequest request, CancellationToken cancellationToken = default)
        {
            var command = new UpdateProficiencyLevelCommand(studentId, skillId, request.Proficiency);
            await _updateProficiencyLevelCommandHandler.Handle(command, cancellationToken);
            return NoContent();
        }

        [HttpDelete("{studentId:guid}/skills/{skillId:guid}")]
        public async Task<IActionResult> DeleteSkill(Guid studentId, Guid skillId, CancellationToken cancellationToken = default)
        {
            var command = new DeleteStudentSkillCommand(studentId, skillId, cancellationToken);
            await _deleteStudentSkillCommandHandler.Handle(command, cancellationToken);
            return NoContent();
        }
    }
}
