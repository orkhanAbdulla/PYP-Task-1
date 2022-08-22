using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;
using System.Net;
using System.Net.Mime;
using ExcelUploadReadDataSave.Application.CustomExceptions;


namespace ExcelUploadReadDataSave.Api.ServiceExtentions
{
    public static class ExceptionHandlerService
    {
        public static void AddExceptionHandlerExtention<T>(this IApplicationBuilder app,ILogger<T> logger)
        {
            app.UseExceptionHandler(error =>
            {
                error.Run(async context =>
                {
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    var code = 500;
                    string message = "Inter Server Error. Please Try Again Later!";

                    if (contextFeature != null)
                    {

                        message = contextFeature.Error.Message;
                         if (contextFeature.Error is System.IO.FileFormatException)
                        {
                            code = 400;
                        }
                        else if (contextFeature.Error is NotReportTypeException)
                        {
                            code = 400;
                        }
                        else if (contextFeature.Error is DateIntervalException)
                        {
                            code = 400;
                        }
                        //else if (contextFeature.Error is RecordAlreadyExistException)
                        //{
                        //    code = 409;
                        //}
                        logger.LogError(contextFeature.Error.Message);

                    }
                    context.Response.StatusCode = code;
                    var JsonStr = JsonConvert.SerializeObject(new
                    {
                        code = code,
                        message = message
                    });
                    await context.Response.WriteAsync(JsonStr);
                });
            });

        }
    }
}
