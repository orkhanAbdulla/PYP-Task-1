using ExcelUploadReadDataSave.Api.ServiceExtentions;
using ExcelUploadReadDataSave.Application;
using ExcelUploadReadDataSave.Infrastructure;
using ExcelUploadReadDataSave.Persistence;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System.Collections.ObjectModel;
using System.Data;
using System.Security.Claims;
using static ExcelUploadReadDataSave.Application.Utilis.Enums;
using static Serilog.Sinks.MSSqlServer.ColumnOptions;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddPersistenceServices();

var columnOptions = new ColumnOptions
{
    AdditionalColumns = new Collection<SqlColumn>
    {
        new SqlColumn
            {ColumnName = "Email", PropertyName = "Email", DataType = SqlDbType.NVarChar},

        new SqlColumn
            {ColumnName = "ReportType",PropertyName = "ReportType", DataType = SqlDbType.NVarChar},
    }
};

Logger log = new LoggerConfiguration()
    .WriteTo.File("logs/log.txt")
    .WriteTo.MSSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), "logs",
    autoCreateSqlTable: true,
     restrictedToMinimumLevel: LogEventLevel.Verbose,
    columnOptions: columnOptions
    )
    .Enrich.FromLogContext()
    .MinimumLevel.Information()
    .CreateLogger();

builder.Host.UseSerilog(log);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSerilogRequestLogging();
app.AddExceptionHandlerExtention<Program>(app.Services.GetRequiredService<ILogger<Program>>());
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseAuthorization();
app.Use(async (httpContext, next) =>
{
    var email = httpContext.Request.Query["toMail"].ToString();
    var reportType = (ReportType)Int32.Parse(httpContext.Request.Query["Type"].ToString());
    LogContext.PushProperty("Email", email);
    LogContext.PushProperty("ReportType", reportType);

    await next.Invoke();
}
);
app.MapControllers();

app.Run();
