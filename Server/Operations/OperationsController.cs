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
    public class OperationsController : ControllerBase
    {
        private readonly OperationPipeline _pipeline;

        public OperationsController(OperationPipeline pipeline)
        {
            _pipeline = pipeline;
        }

        [HttpPost]
        [TypeFilter(typeof(OperationValidationFilter))]
        [AuthorizationFilter]
        public async Task<IActionResult> Perform([FromBody] OperationDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var operation = HttpContext.Items[OperationValidationFilter.OperationKey] as Message;
            var result = await _pipeline.ExecuteAsync(dto.Id, dto.Flags, operation);

            return Ok(result);
        }
    }
}
