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
        private readonly OperationPipeline _pipeline;

        public OperationsController(IContractRegistry contracts, OperationPipeline pipeline)
        {
            _contracts = contracts;
            _pipeline = pipeline;
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
            var result = await _pipeline.ExecuteAsync(dto.Id, dto.Flags, initialOperation);

            // output validation
            if ((contract.ReturnType.Equals(typeof(void)) && result != null)
                || result == null || !contract.ReturnType.Equals(result.GetType())) {
                return Problem("The return data is invalid.");
            }

            return Ok(result);
        }
    }
}
