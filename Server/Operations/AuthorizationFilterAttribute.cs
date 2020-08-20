using Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json.Linq;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server
{
    public class AuthorizationFilterAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Items.TryGetValue(
                OperationValidationFilter.OperationContractKey, out object contractObj))
            {
                var contract = contractObj as OperationContract;
                var clientRole = (Role)context.HttpContext.Items[CookieIdentificationMiddleware.ClientRoleKey];
                if (contract.Role > clientRole)
                {
                    context.Result = new ContentResult
                    {
                        StatusCode = StatusCodes.Status403Forbidden,
                        Content = $"{contract.Role} role is required."
                    };
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
