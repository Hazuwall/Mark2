using Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Operations
{
    public class OperationRequestHandler
    {
        private readonly IContractRegistry _contracts;
        private readonly OperationPipeline _pipeline;
        private readonly JsonSerializer _serializer = new JsonSerializer();

        public OperationRequestHandler(IContractRegistry contracts, OperationPipeline pipeline)
        {
            _contracts = contracts;
            _pipeline = pipeline;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var request = context.Request;
            var response = context.Response;

            var title = (string)request.RouteValues["title"];
            if (!_contracts.TryGetOperationContract(title, out OperationContract contract))
            {
                response.StatusCode = StatusCodes.Status404NotFound;
                return;
            }

            var clientRole = (Role)context.Items[CookieIdentificationMiddleware.ClientRoleKey];
            if (contract.Role > clientRole)
            {
                response.StatusCode = StatusCodes.Status403Forbidden;
                response.ContentType = "text/plain";
                await response.WriteAsync($"{contract.Role} role is required.");
                return;
            }

            var payload = await request.Body.ReadJsonPayloadAsync(contract.ParameterType, _serializer);
            if(payload == null && !contract.ParameterType.Equals(typeof(void)))
            {
                response.StatusCode = StatusCodes.Status400BadRequest;
                return;
            }
            Guid.TryParse(request.Query["id"], out Guid id);

            var result = await _pipeline.ExecuteAsync(new Message(title, payload), id);
            if (result != null)
            {
                response.ContentType = "application/json";
                await response.Body.WriteJsonPayloadAsync(result, _serializer);
            }
        }
    }
}
