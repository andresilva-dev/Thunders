using FluentValidation;
using Thunders.TechTest.ApiService.Dto;

namespace Thunders.TechTest.ApiService.Validators
{
    public class ResultReportDtoValidator : AbstractValidator<ResultReportDto>
    {
        public ResultReportDtoValidator()
        {
            RuleFor(x => x.RequestReportId)
                .NotEmpty()
                .WithMessage("O ID da requisição de relatório é obrigatório.");
        }
    }
}
