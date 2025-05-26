using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Thunders.TechTest.ApiService.Dto;
using Thunders.TechTest.ApiService.Services.Interfaces;

namespace Thunders.TechTest.ApiService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TollStationController : ControllerBase
    {
        private readonly ITollStationService _tollStationService;
        private readonly IValidator<TollStationDto> _validator;
        private readonly ILogger<TollStationController> _logger;

        public TollStationController(ITollStationService tollStationService, IValidator<TollStationDto> validator, ILogger<TollStationController> logger)
        {
            _tollStationService = tollStationService;
            _validator = validator;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _tollStationService.GetAllAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                var message = "Error occurred while try get all toll station.";
                _logger.LogError(ex, message);
                return StatusCode(500, message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _tollStationService.GetByIdAsync(id);
                return result != null ? Ok(result) : NotFound();
            }
            catch (Exception ex)
            {
                var message = "Error occurred while try get toll station by id.";
                _logger.LogError(ex, message);
                return StatusCode(500, message);
            }
           
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TollStationDto dto)
        {
            ValidationResult validation = await _validator.ValidateAsync(dto);

            if (!validation.IsValid)
            {
                return BadRequest(validation.Errors.Select(e => new
                {
                    Field = e.PropertyName,
                    Erro = e.ErrorMessage
                }));
            }

            try
            {
                var id = await _tollStationService.AddAsync(dto);
                return Ok(new { Id = id});
            }
            catch (Exception ex)
            {
                var message = "Error occurred while creating toll station.";
                _logger.LogError(ex, message);
                return StatusCode(500, message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TollStationDto dto)
        {
            ValidationResult validation = await _validator.ValidateAsync(dto);

            if (!validation.IsValid)
            {
                return BadRequest(validation.Errors.Select(e => new
                {
                    Field = e.PropertyName,
                    Erro = e.ErrorMessage
                }));
            }

            try
            {
                await _tollStationService.UpdateAsync(id, dto);
                return Ok();
            }
            catch (Exception ex)
            {
                const string message = "Error occurred while updating tollstation.";
                _logger.LogError(ex, message);
                return StatusCode(500, message);
            }
        }
    }
}
