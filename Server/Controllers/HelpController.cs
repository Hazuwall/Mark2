using Common;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using Server.Operations;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    public class HelpController : ControllerBase
    {
        private readonly IApiDeclaraionRegistry _registry;

        public HelpController(IApiDeclaraionRegistry registry)
        {
            _registry = registry;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_registry.MessageInfos);
        }
    }
}
