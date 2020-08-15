using System;
using System.Threading.Tasks;
using Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks.Dataflow;
using Server.Operations;
using Serilog;

namespace Server.Controllers
{
    [Route("api/[controller]/[action]")]
    public class OperationController : ControllerBase
    {
        private readonly IContractRegistry _contracts;
        private readonly OperationPipelineBuilder _builder;

        public OperationController(IContractRegistry contracts, OperationPipelineBuilder builder)
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
                return StatusCode(StatusCodes.Status501NotImplemented, "An operation is not registered.");
            }
            Message initialOperation;
            try
            {
                if (contract.InputType.Equals(typeof(void)))
                {
                    initialOperation = new Message(dto.Title);
                }
                else
                {
                    initialOperation = new Message(dto.Title, dto.Payload.ToObject(contract.InputType));
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
                return Unauthorized();
            }

            // processing
            var context = new OperationContext(dto.Id, dto.Flags, initialOperation);
            try
            {
                await _builder.Pipeline.SendAsync(context);
                await Task.Delay(2000);
            }
            catch(Exception ex)
            {
                if (Log.IsEnabled(Serilog.Events.LogEventLevel.Error))
                {
                    Log.Error(ex, "An error occured while processing {0} operation.", context.CurrentOperation);
                }
                return Problem(ex.Message);
            }

            // output validation
            if (context.CurrentOperation != null)
            {
                return StatusCode(StatusCodes.Status417ExpectationFailed, $"An operation {context.CurrentOperation.Title} was not processed.");
            }
            else if ((contract.OutputType.Equals(typeof(void)) && context.Result != null)
                || context.Result == null || !contract.OutputType.Equals(context.Result.GetType())) {
                return StatusCode(StatusCodes.Status417ExpectationFailed, "The output is invalid.");
            }

            return Ok(context.Result);
        }
    }
}
