using Microsoft.AspNetCore.Mvc;
using Thunders.TechTest.ApiService.Services.Interfaces;

namespace Thunders.TechTest.ApiService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StateController : ControllerBase
    {
        private readonly IStateService _stateService;
        private readonly ILogger<StateController> _logger;

        public StateController(IStateService stateService, ILogger<StateController> logger)
        {
            _stateService = stateService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _stateService.GetAllAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                var message = "Error ocurred during try to get all states.";

                _logger.LogError(ex, message);
                return StatusCode(500, message);
            }
            
        }
    }
}
