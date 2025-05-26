using FluentValidation;
using Thunders.TechTest.ApiService.Dto;

namespace Thunders.TechTest.ApiService.Validators
{
    public class TopStationsReportRequestDtoValidator : AbstractValidator<TopStationsReportRequestDto>
    {
        public TopStationsReportRequestDtoValidator()
        {
            RuleFor(x => x.QtdToProcess)
                .GreaterThan(0).WithMessage("A quantidade a processar deve ser maior que zero.");

            RuleFor(x => x.Month)
                .InclusiveBetween(1, 12).WithMessage("O mês deve estar entre 1 e 12.");

            RuleFor(x => x.Year)
                .LessThanOrEqualTo(DateTime.UtcNow.Year).WithMessage("O ano não pode ser maior que o ano atual.");

            RuleFor(x => x)
                .Must(BeAValidYearMonth)
                .WithMessage("O mês e ano não podem estar no futuro.");
        }

        private bool BeAValidYearMonth(TopStationsReportRequestDto dto)
        {
            var now = DateTime.UtcNow;
            var requested = new DateTime(dto.Year, dto.Month, 1);
            var current = new DateTime(now.Year, now.Month, 1);

            return requested <= current;
        }
    }
}
