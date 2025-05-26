using FluentValidation;
using Thunders.TechTest.ApiService.Dto;

namespace Thunders.TechTest.ApiService.Validators
{
    public class TollStationDtoValidator : AbstractValidator<TollStationDto>
    {
        public TollStationDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("O nome é obrigatório.")
                .MinimumLength(3).WithMessage("O nome deve ter pelo menos 3 caracteres.");
        }
    }
}
