using ExcelUploadReadDataSave.Application.Services;
using ExcelUploadReadDataSave.Infrastructure.Services;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SendGrid.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelUploadReadDataSave.Infrastructure
{
    public static class ServicesRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddSendGrid(option =>
            {
                option.ApiKey = Configuration.SendGridEmailSetting("SendGridEmailSetting:APIKey");
             });
            services.AddScoped<IMailService, MailService>();
        }
    }
}
