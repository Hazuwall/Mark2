using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    var error = context.Features.Get<IExceptionHandlerFeature>()?.Error;
                    if (error != null)
                    {
                        if (Log.IsEnabled(Serilog.Events.LogEventLevel.Error))
                        {
                            Log.Error(error, "An error occured.");
                        }

                        if (error is NotImplementedException)
                        {
                            context.Response.StatusCode = StatusCodes.Status501NotImplemented;
                        }
                        else
                        {
                            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                            context.Response.ContentType = "text/plain";
                            await context.Response.WriteAsync(error.Message);
                        }
                    }
                });
            });
        }
    }
}
