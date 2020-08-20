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
    public class ContractsController : ControllerBase
    {
        private readonly IContractRegistry _contracts;

        public ContractsController(IContractRegistry contracts)
        {
            _contracts = contracts;
        }

        [HttpGet]
        public IActionResult Operations()
        {
            return Ok(_contracts.OperationContracts);
        }

        [HttpGet]
        public IActionResult Data()
        {
            return Ok(_contracts.DataContracts);
        }

        [HttpGet]
        public IActionResult Events()
        {
            return Ok(_contracts.EventContracts);
        }
    }
}
