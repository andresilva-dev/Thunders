using FluentValidation;
using Thunders.TechTest.ApiService.Dto;

namespace Thunders.TechTest.ApiService.Validators
{
    public class CityDtoValidator : AbstractValidator<CityDto>
    {
        public CityDtoValidator()
        {
            RuleFor(x => x.StateId)
                .InclusiveBetween(1, 27)
                .WithMessage("O ID do estado deve estar entre 1 e 27.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("O nome é obrigatório.")
                .MinimumLength(3).WithMessage("O nome deve ter pelo menos 3 caracteres.");
        }
    }
}
