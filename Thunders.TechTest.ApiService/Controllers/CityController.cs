using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Thunders.TechTest.ApiService.Dto;
using Thunders.TechTest.ApiService.Services.Interfaces;

namespace Thunders.TechTest.ApiService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CityController : ControllerBase
    {
        private readonly ICityService _cityService;
        private readonly IValidator<CityDto> _validator;
        private readonly ILogger<CityController> _logger;

        public CityController(ICityService cityService, IValidator<CityDto> validator, ILogger<CityController> logger)
        {
            _cityService = cityService;
            _validator = validator;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _cityService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _cityService.GetByIdAsync(id);
            return result != null ? Ok(result) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create( [FromBody] CityDto cityDto)
        {
            ValidationResult validation = await _validator.ValidateAsync(cityDto);

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
                await _cityService.AddAsync(cityDto);
                return Created();
            }
            catch (Exception ex)
            {
                const string message = "Error occurred while creating city.";
                _logger.LogError(ex, message);
                return StatusCode(500, message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CityDto cityDto)
        {
            ValidationResult validation = await _validator.ValidateAsync(cityDto);

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
                await _cityService.UpdateAsync(id, cityDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                const string message = "Error occurred while updating city.";
                _logger.LogError(ex, message);
                return StatusCode(500, message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _cityService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                const string message = "Error occurred while updating city.";
                _logger.LogError(ex, message);
                return StatusCode(500, message);
            }
        }
    }
}
