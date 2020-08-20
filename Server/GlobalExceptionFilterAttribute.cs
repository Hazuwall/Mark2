using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server
{
    public class GlobalExceptionFilterAttribute : Attribute, IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (Log.IsEnabled(Serilog.Events.LogEventLevel.Error))
            {
                Log.Error(context.Exception, "An error occured.");
            }
            context.Result = new ContentResult
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Content = context.Exception.Message
            };
            context.ExceptionHandled = true;
        }
    }
}
