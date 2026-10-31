using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentSkillManagement.Application.Features.Students;
using StudentSkillManagement.Application.Features.Students.Commands.CreateStudent;
using StudentSkillManagement.Presentation.Contracts.Students;
using System.Reflection.Metadata;

namespace StudentSkillManagement.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly CreateStudentCommandHandler _createStudentCommandHandler;
        public StudentsController(CreateStudentCommandHandler createStudentCommandHandler)
        {
            _createStudentCommandHandler = createStudentCommandHandler;
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
        public IActionResult GetById(Guid id)
        {
            // We'll implement the retrieval logic later.
            return Ok();
        }
    }
}
