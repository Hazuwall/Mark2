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
    public class OperationValidationFilter : IActionFilter
    {
        public static readonly string OperationKey = "Operation";
        public static readonly string OperationContractKey = "OperationContract";

        private readonly IContractRegistry _contractRegistry;
        private OperationContract _contract = null;

        public OperationValidationFilter(IContractRegistry contractRegistry)
        {
            _contractRegistry = contractRegistry;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var title = (string)context.ActionArguments["title"];
            var payload = (JToken)context.ActionArguments["payload"];

            if (!_contractRegistry.TryGetOperationContract(title, out _contract))
            {
                context.ModelState.AddModelError(nameof(title), "An operation is not registered.");
                return;
            }
            context.HttpContext.Items.Add(OperationContractKey, _contract);

            if(TryConvertToValidOperation(title, payload, _contract, out Message operation))
            {
                context.HttpContext.Items.Add(OperationKey, operation);
            }
            else
            {
                context.ModelState.AddModelError(nameof(payload), "Parameter data is invalid.");
                return;
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (_contract != null && context.Result is OkObjectResult okResult && !IsReturnValid(okResult.Value, _contract))
            {
                context.Result = new ContentResult
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Content = "Return data is invalid."
                };
            }
        }

        public bool TryConvertToValidOperation(string title, JToken payload, OperationContract contract, out Message operation)
        {
            if (contract.ParameterType.Equals(typeof(void)))
            {
                operation = new Message(title);
            }
            else
            {
                try
                {
                    operation = new Message(title, payload.ToObject(contract.ParameterType));
                }
                catch
                {
                    operation = null;
                    return false;
                }
            }
            return true;
        }

        public bool IsReturnValid(object result, OperationContract contract)
        {
            if (contract.ReturnType.Equals(typeof(void)))
            {
                return result == null;
            }
            else
            {
                return contract.ReturnType.Equals(result.GetType());
            }
        }
    }
}
