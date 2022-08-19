﻿using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;
using System.Net;
using System.Net.Mime;
using ExcelUploadReadDataSave.Application.CustomExceptions;


namespace ExcelUploadReadDataSave.Api.ServiceExtentions
{
    public static class ExceptionHandlerService
    {
        public static void AddExceptionHandlerExtention(this IApplicationBuilder app)
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
                         if (contextFeature.Error is FileFormatException)
                        {
                            code = 400;
                        }
                        //else if (contextFeature.Error is ItemNotFoundException)
                        //{
                        //    code = 404;
                        //}
                        //else if (contextFeature.Error is RecordAlreadyExistException)
                        //{
                        //    code = 409;
                        //}
                    
                        //else if (contextFeature.Error is ItemNotFoundException)
                        //{
                        //    code = 400;
                        //}
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
