using Azure.Core;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Thunders.TechTest.ApiService.Dto;
using Thunders.TechTest.ApiService.Services.Interfaces;

namespace Thunders.TechTest.ApiService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;
        private readonly IValidator<TopStationsReportRequestDto> _validatorTopStations;
        private readonly IValidator<VehicleTypesByStationRequestDto> _validatorVehicleTypes;
        private readonly IValidator<ResultReportDto> _validatorResultReport;
        private readonly ILogger<ReportsController> _logger;

        public ReportsController(IReportService reportService, IValidator<TopStationsReportRequestDto> validatorTopStations,
            IValidator<VehicleTypesByStationRequestDto> validatorVehicleTypes, IValidator<ResultReportDto> validatorResultReport, ILogger<ReportsController> logger)
        {
            _reportService = reportService;
            _validatorTopStations = validatorTopStations;
            _validatorVehicleTypes = validatorVehicleTypes;
            _validatorResultReport = validatorResultReport; 
            _logger = logger;
        }

        [HttpPost("generate/total-hour-city")]
        public async Task<IActionResult> GenerateTotalPerHourPerCityReport()
        {
            var reportType = "totalPerHourPerCity";

            var requestId = await _reportService.ExecuteAsync(reportType, null);

            return Accepted(new { requestId });
        }

        [HttpPost("generate/top-stations-month")]
        public async Task<IActionResult> GenerateTopStationsReport([FromBody] TopStationsReportRequestDto dto)
        {
            ValidationResult validationResult = await _validatorTopStations.ValidateAsync(dto);

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
                var reportType = "topStationsByMonth";

                var parameters = new Dictionary<string, string>
                {
                    ["top"] = dto.QtdToProcess.ToString(),
                    ["month"] = dto.Month.ToString(),
                    ["year"] = dto.Year.ToString(),
                };

                var requestId = await _reportService.ExecuteAsync(reportType, parameters);

                return Accepted(new { requestId });
            }
            catch (Exception ex)
            {
                var message = "Error ocurred during try request report.";

                _logger.LogError(ex, message);
                return StatusCode(500, message);
            }
            
        }

        [HttpPost("generate/vehicle-types-by-station")]
        public async Task<IActionResult> GenerateVehicleTypesReport([FromBody] VehicleTypesByStationRequestDto dto)
        {
            ValidationResult validationResult = await _validatorVehicleTypes.ValidateAsync(dto);

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
                var reportType = "vehiclesByStation";

                var parameters = new Dictionary<string, string>
                {
                    ["stationId"] = dto.StationId.ToString()
                };

                var requestId = await _reportService.ExecuteAsync(reportType, parameters);

                return Accepted(new { requestId });
            }
            catch (Exception ex)
            {
                var message = "Error ocurred during try request report.";

                _logger.LogError(ex, message);
                return StatusCode(500, message);
            }
            
        }

        [HttpPost("result")]
        public async Task<IActionResult> GetResult([FromBody] ResultReportDto dto)
        {
            ValidationResult validationResult = await _validatorResultReport.ValidateAsync(dto);

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
                var result = await _reportService.GetResult(dto);
                return result == null ? NotFound() : Ok(result);
            }
            catch (Exception ex)
            {
                var message = "Error ocurred during try request report.";

                _logger.LogError(ex, message);
                return StatusCode(500, message);
            }
        }
    }
}
