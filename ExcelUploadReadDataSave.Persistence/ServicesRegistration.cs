using ExcelUploadReadDataSave.Application.Repositories;
using ExcelUploadReadDataSave.Application.Services;
using ExcelUploadReadDataSave.Infrastructure.Services;
using ExcelUploadReadDataSave.Persistence.Context;
using ExcelUploadReadDataSave.Persistence.Repositories;
using ExcelUploadReadDataSave.Persistence.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelUploadReadDataSave.Persistence
{
    public static class ServicesRegistration
    {
        public static void AddPersistenceServices(this IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(Configuration.ConnectionString));
            services.AddScoped<IReportRepository, ReportRepository>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IFileManager, FileManager>();

        }
    }
}
