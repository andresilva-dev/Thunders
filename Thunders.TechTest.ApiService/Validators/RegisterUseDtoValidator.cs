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

            RuleFor(x => x.AmountPaid)
                .GreaterThan(0).WithMessage("O valor pago deve ser maior que zero.");

            RuleFor(x => x.VehicleType)
                .IsInEnum().WithMessage("O tipo de veículo é inválido.");

            RuleFor(x => x.UsedAt)
            .Must(BeInThePast).WithMessage("A data de uso deve ser anterior à data/hora atual.");
        }

        private bool BeInThePast(DateTime usedAt)
        {
            return usedAt <= DateTime.UtcNow;
        }
    }
}
