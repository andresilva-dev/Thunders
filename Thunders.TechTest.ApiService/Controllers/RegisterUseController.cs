using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Thunders.TechTest.ApiService.Dto;
using Thunders.TechTest.ApiService.Services.Interfaces;

namespace Thunders.TechTest.ApiService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegisterUseController : ControllerBase
    {
        private readonly IValidator<RegisterUseDto> _validator;
        private readonly IRegisterUseService _registerUseService;
        private readonly ILogger<RegisterUseController> _logger;

        public RegisterUseController(
            IValidator<RegisterUseDto> validator,
            IRegisterUseService registerUseService,
            ILogger<RegisterUseController> logger)
        {
            _validator = validator;
            _registerUseService = registerUseService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRegistereUse([FromBody] RegisterUseDto dto)
        {
            ValidationResult validationResult = await _validator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => new
                {
                    Field = e.PropertyName,
                    Erro = e.ErrorMessage
                }));
            }

            try
            {
                var id = await _registerUseService.AddRegisterUseAsync(dto);
                return Ok(new { Id = id });
            }
            catch (Exception ex)
            {
                var message = "Error ocurred during try add register.";

                _logger.LogError(ex, message);
                return StatusCode(500, message);
            }
        }
    }
}




