using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentSkillManagement.Application.Features.Skills.Queries.GetAllSkills;

namespace StudentSkillManagement.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkillsController : ControllerBase
    {
        private GetAllSkillsQueryHandler _getAllSkillsQueryHandler;

        public SkillsController(GetAllSkillsQueryHandler getAllSkillsQueryHandler)
        {
            _getAllSkillsQueryHandler = getAllSkillsQueryHandler;
        }

        [HttpGet]
        public async Task<IActionResult>GetAll(CancellationToken cancellationToken= default)
        {
            var query = new GetAllSkillsQuery();

            var skillDetailsDtos = await _getAllSkillsQueryHandler.Handle(query, cancellationToken);

            return Ok(skillDetailsDtos);
        }
    }
}
