using FluentValidation;
using Thunders.TechTest.ApiService.Dto;

namespace Thunders.TechTest.ApiService.Validators
{
    public class VehicleTypesByStationRequestDtoValidator : AbstractValidator<VehicleTypesByStationRequestDto>
    {
        public VehicleTypesByStationRequestDtoValidator()
        {
            RuleFor(x => x.StationId)
                .GreaterThan(0)
                .WithMessage("O ID da praça de pedágio deve ser maior que zero.");
        }
    }
}
