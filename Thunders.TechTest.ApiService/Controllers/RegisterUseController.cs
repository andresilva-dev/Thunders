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
        public async Task<IActionResult> RegistrarUtilizacao([FromBody] RegisterUseDto dto)
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
                await _registerUseService.AddRegisterUseAsync(dto);
                return Created();
            }
            catch (Exception ex)
            {
                var message = "Error ocurred during try add register.";

                _logger.LogError(ex, message);
                return StatusCode(500, message);
            }
        }

        /// <summary>
        /// Valor total por hora por cidade
        /// </summary>
        [HttpGet("relatorio/valor-total-por-hora-cidade")]
        public async Task<IActionResult> GetTotalByHourAndCity()
        {
            try
            {
                //var result = await _registerUseService.GetTotalAmountPerHourByCityAsync();
                //return Ok(result);
                return Ok();
            }
            catch (Exception ex)
            {
                var message = "Error ocurred during try get total by hour and city.";

                _logger.LogError(ex, message);
                return StatusCode(500, message);
            }
            
        }

        /// <summary>
        /// Praças que mais faturaram por mês
        /// </summary>
        [HttpGet("relatorio/top-pracas-faturamento")]
        public async Task<IActionResult> GetTopTollStationsByMonth([FromQuery] int month, [FromQuery] int year, [FromQuery] int top = 5)
        {
            try
            {
                //var result = await _registerUseService.GetTopTollStationsByMonthAsync(month, year, top);
                //return Ok(result);
                return Ok();
            }
            catch (Exception ex)
            {
                var message = "Error ocurred during try get top tollstations by mounth.";

                _logger.LogError(ex, message);
                return StatusCode(500, message);
            }
        }

        /// <summary>
        /// Tipos de veículos que passaram por uma praça
        /// </summary>
        [HttpGet("relatorio/tipos-veiculos-por-praca")]
        public async Task<IActionResult> GetQtdVehicleTypesByTollStation([FromQuery] string tollStationName)
        {
            try
            {
                //var result = await _registerUseService.GetQtdVehicleTypesByTollStationAsync(tollStationName);
                //return Ok(result);
                return Ok();
            }
            catch (Exception ex)
            {
                var message = "Error ocurred during try get quantity vehicle types by tollstations by mounth.";

                _logger.LogError(ex, message);
                return StatusCode(500, message);
            }
            
        }
    }

}




