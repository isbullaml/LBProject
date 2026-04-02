using Core.Entities;
using Core.Interfaces;
using LBProject.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LBProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BreachesController : ControllerBase
    {
        private IBreachRepository _breachRepository;
        public BreachesController(IBreachRepository breachRepository)
        {
            _breachRepository = breachRepository;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Breach>>> GetAll()
        {
            IEnumerable<Breach> breaches = await _breachRepository.GetAllAsync();
            List<BreachDto> result = BreachDto.FromEntityList(breaches);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BreachDto>> GetByid(string id)
        {
            Breach breach = await _breachRepository.GetByIdAsync(id);
            if (breach == null)
            {
                return NotFound();
            }
            BreachDto dto = BreachDto.FromEntity(breach);
            return Ok(dto);
        }
    }
}
