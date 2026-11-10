using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentSkillManagement.Application.Features.Skills.Commands.ChangeSkillStatus;
using StudentSkillManagement.Application.Features.Skills.Commands.CreateSkill;
using StudentSkillManagement.Application.Features.Skills.Commands.UpdateSkill;
using StudentSkillManagement.Application.Features.Skills.Queries.GetAllSkills;
using StudentSkillManagement.Application.Features.Skills.Queries.GetSkillById;
using StudentSkillManagement.Presentation.Contracts.Skills;

namespace StudentSkillManagement.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkillsController : ControllerBase
    {
        private GetAllSkillsQueryHandler _getAllSkillsQueryHandler;
        private CreateSkillCommandHandler _createSkillCommandHandler;
        private GetSkillByIdQueryHandler _getSkillByIdQueryHandler;
        private UpdateSkillCommandHandler _updateSkillCommandHandler;
        private ChangeSkillStatusCommandHandler _changeSkillStatusCommandHandler;

        public SkillsController(GetAllSkillsQueryHandler getAllSkillsQueryHandler,
            CreateSkillCommandHandler createSkillCommandHandler, GetSkillByIdQueryHandler getSkillByIdQueryHandler,
            UpdateSkillCommandHandler updateSkillCommandHandler, ChangeSkillStatusCommandHandler changeSkillStatusCommandHandler)
        {
            _getAllSkillsQueryHandler = getAllSkillsQueryHandler;
            _createSkillCommandHandler = createSkillCommandHandler;
            _getSkillByIdQueryHandler = getSkillByIdQueryHandler;
            _updateSkillCommandHandler = updateSkillCommandHandler;
            _changeSkillStatusCommandHandler = changeSkillStatusCommandHandler;
        }

        [HttpGet]
        public async Task<IActionResult>GetAll(CancellationToken cancellationToken= default)
        {
            var query = new GetAllSkillsQuery();

            var skillDetailsDtos = await _getAllSkillsQueryHandler.Handle(query, cancellationToken);

            return Ok(skillDetailsDtos);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSkillRequest request, CancellationToken cancellationToken)
        {
            var command = new CreateSkillCommand(
                request.Name,
                request.Description);

            var skillId = await _createSkillCommandHandler.Handle(
                command,
                cancellationToken);

            return CreatedAtAction(
                nameof(GetById),
                new { id = skillId },
                new { id = skillId });
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
        {
            var query = new GetSkillByIdQuery(id);

            var skill = await _getSkillByIdQueryHandler.Handle(
                query,
                cancellationToken);

            if (skill == null)
                return NotFound();

            return Ok(skill);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateSkillRequest request,
            CancellationToken cancellationToken= default)
        {
            var command = new UpdateSkillCommand(id, request.Name, request.Description);
            await _updateSkillCommandHandler.Handle(command, cancellationToken);
            return NoContent();
        }

        [HttpPatch("{id:guid}/status")]
        public async Task<IActionResult> ChangeStatus(Guid id, [FromBody] ChangeSkillStatusRequest request, CancellationToken cancellationToken=default)
        {
            var command = new ChangeSkillStatusCommand(id, request.IsActive);
            await _changeSkillStatusCommandHandler.Handle(command, cancellationToken);
            return NoContent();
        }
    }
}
