using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentSkillManagement.Application.Features.Students;
using StudentSkillManagement.Application.Features.Students.Commands.ChangeStudentStatus;
using StudentSkillManagement.Application.Features.Students.Commands.CreateStudent;
using StudentSkillManagement.Application.Features.Students.Commands.UpdateStudentProfile;
using StudentSkillManagement.Application.Features.Students.Queries.GetAllStudents;
using StudentSkillManagement.Application.Features.Students.Queries.GetStudentById;
using StudentSkillManagement.Presentation.Contracts.Students;
using System.Reflection.Metadata;

namespace StudentSkillManagement.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly CreateStudentCommandHandler _createStudentCommandHandler;
        private readonly GetStudentByIdQueryHandler _getStudentByIdQueryHandler;
        private readonly GetAllStudentsQueryHandler _getAllStudentsQueryHandler;
        private readonly UpdateStudentProfileCommandHandler _updateStudentProfileCommandHandler;
        private readonly ChangeStudentStatusCommandHandler _changeStudentStatusCommandHandler;
        public StudentsController(CreateStudentCommandHandler createStudentCommandHandler, GetStudentByIdQueryHandler getStudentByIdQueryHandler,
            GetAllStudentsQueryHandler getAllStudentsQueryHandler, UpdateStudentProfileCommandHandler updateStudentProfileCommandHandler,
            ChangeStudentStatusCommandHandler changeStudentStatusCommandHandler)
        {
            _createStudentCommandHandler = createStudentCommandHandler;
            _getStudentByIdQueryHandler = getStudentByIdQueryHandler;
            _getAllStudentsQueryHandler = getAllStudentsQueryHandler;
            _updateStudentProfileCommandHandler = updateStudentProfileCommandHandler;
            _changeStudentStatusCommandHandler = changeStudentStatusCommandHandler;
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
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllStudentsQuery();
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
    }
}
