using System;
using System.Threading.Tasks;
using Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks.Dataflow;
using Server.Operations;
using Serilog;
using Newtonsoft.Json.Linq;

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
        [Route("api/operations/{title}")]
        [TypeFilter(typeof(OperationValidationFilter))]
        [AuthorizationFilter]
        public async Task<IActionResult> Perform(string title, Guid id, [FromBody] JToken payload=null)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var operation = HttpContext.Items[OperationValidationFilter.OperationKey] as Message;
            var result = await _pipeline.ExecuteAsync(id, operation);

            return Ok(result);
        }
    }
}
