using ExcelUploadReadDataSave.Application.DTOs.ReportDto;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelUploadReadDataSave.Application.Validators
{
    public class SendReportDtoValidator: AbstractValidator<SendReportDto>
    {
        public SendReportDtoValidator()
        {
            RuleFor(d => d.EndDate).NotEmpty()
          .GreaterThan(r => r.StartDate)
          .WithMessage("Start Date is not greater than End Date");
            RuleFor(x => x.Type).NotNull();
           RuleForEach(x => x.emailAddresses).Matches("^[a-z0-9]+(?!.*(?:\\+{2,}|\\-{2,}|\\.{2,}))(?:[\\.+\\-]{0,1}[a-z0-9])*@code.edu\\.az$");
        }
    }
}
