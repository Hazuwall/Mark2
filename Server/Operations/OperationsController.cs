using System;
using System.Threading.Tasks;
using Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks.Dataflow;
using Server.Operations;
using Serilog;

namespace Server.Operations
{
    [Route("api/[controller]/[action]")]
    public class OperationsController : ControllerBase
    {
        private readonly IContractRegistry _contracts;
        private readonly OperationPipelineBuilder _builder;

        public OperationsController(IContractRegistry contracts, OperationPipelineBuilder builder)
        {
            _contracts = contracts;
            _builder = builder;
        }

        [HttpPost]
        public async Task<IActionResult> Perform([FromBody] OperationDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            // input validation
            if (!_contracts.TryGetOperationContract(dto.Title, out OperationContract contract))
            {
                return NotFound("An operation is not registered.");
            }
            Message initialOperation;
            try
            {
                if (contract.ParameterType.Equals(typeof(void)))
                {
                    initialOperation = new Message(dto.Title);
                }
                else
                {
                    initialOperation = new Message(dto.Title, dto.Payload.ToObject(contract.ParameterType));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            // authorization
            var clientRole = (Role)HttpContext.Items[CookieIdentificationMiddleware.ClientRoleKey];
            if(contract.Role > clientRole)
            {
                return Forbid($"{contract.Role} role is required.");
            }

            // processing
            var context = new OperationContext(dto.Id, dto.Flags, initialOperation);
            await _builder.Pipeline.SendAsync(context);
            await Task.Delay(500);

            // output validation
            if (!context.IsCompleted)
            {
                return Problem($"An operation {context.CurrentOperation.Title} was not processed.");
            }
            else if ((contract.ReturnType.Equals(typeof(void)) && context.Result != null)
                || context.Result == null || !contract.ReturnType.Equals(context.Result.GetType())) {
                return Problem("The return data is invalid.");
            }

            return Ok(context.Result);
        }
    }
}
