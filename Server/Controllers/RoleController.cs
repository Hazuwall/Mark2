using Common;
using Microsoft.AspNetCore.Mvc;
using Server.Roles;
using System;
using System.Collections.Generic;

namespace Server.Controllers
{
    [Route("api/[controller]/[action]")]
    public class RoleController : ControllerBase
    {
        private readonly IClientRoleRegistry _registry;
        private readonly IRoleDisputeFactory _disputeFactory;
        private readonly Dictionary<Role, IRoleDispute> _disputes;

        public RoleController(IClientRoleRegistry registry, IRoleDisputeFactory disputeFactory)
        {
            _registry = registry;
            _disputeFactory = disputeFactory;
            _disputes = new Dictionary<Role, IRoleDispute>()
            {
                { Role.Writer, null },
                { Role.Admin, null }
            };
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok((Role)HttpContext.Items[CookieIdentificationMiddleware.ClientRoleKey]);
        }

        [HttpPost]
        public IActionResult Claim([FromBody] Role requestedRole)
        {
            var client = (Guid)HttpContext.Items[CookieIdentificationMiddleware.ClientIdKey];
            var currentRole = _registry.GetRole(client);
            var owner = _registry.GetOwner(requestedRole);
            if (currentRole == requestedRole)
            {
                return Ok(TryDefend(client, requestedRole));
            }
            else if (!owner.HasValue)
            {
                _registry.SetClientRole(client, requestedRole);
                return Ok(true);
            }
            else
            {
                TryOpenDispute(client, owner.Value, requestedRole);
                return Ok(false);
            }
        }

        private bool TryOpenDispute(Guid claimant, Guid defendant, Role role)
        {
            var currentDispute = _disputes[role];
            if (currentDispute == null || currentDispute.IsResolved)
            {
                var dispute = _disputeFactory.Create(claimant, defendant, role);
                dispute.Open(2000);
                _disputes[role] = dispute;
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool TryDefend(Guid client, Role role)
        {
            var currentDispute = _disputes[role];
            if (currentDispute != null && currentDispute.Defendant == client)
            {
                _disputes[role] = null;
                currentDispute.Defend();
                return currentDispute.IsResolved && currentDispute.IsDefended;
            }
            return true;
        }
    }
}
