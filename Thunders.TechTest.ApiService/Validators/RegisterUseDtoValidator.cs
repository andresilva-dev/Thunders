using FluentValidation;
using Thunders.TechTest.ApiService.Dto;

namespace Thunders.TechTest.ApiService.Validators
{
    public class RegisterUseDtoValidator : AbstractValidator<RegisterUseDto>
    {
        public RegisterUseDtoValidator()
        {
            RuleFor(x => x.TollStationId)
                .GreaterThan(0).WithMessage("O ID da praça de pedágio é obrigatório e deve ser maior que zero.");

            RuleFor(x => x.CityId)
                .GreaterThan(0).WithMessage("O ID da cidade é obrigatório e deve ser maior que zero.");

            RuleFor(x => x.StateId)
                .InclusiveBetween(1, 27)
                .WithMessage("O ID do estado deve estar entre 1 e 27.");

            RuleFor(x => x.AmountPaid)
                .GreaterThan(0).WithMessage("O valor pago deve ser maior que zero.");

            RuleFor(x => x.VehicleType)
                .IsInEnum().WithMessage("O tipo de veículo é inválido.");
        }
    }
}
