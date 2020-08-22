using Common;
using Microsoft.AspNetCore.Mvc;
using Server.Roles;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Roles
{
    public class RoleController : ControllerBase
    {
        private readonly IClientRoleRegistry _registry;
        private readonly IRoleDisputeFactory _disputeFactory;

        public RoleController(IClientRoleRegistry registry, IRoleDisputeFactory disputeFactory)
        {
            _registry = registry;
            _disputeFactory = disputeFactory;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return Ok((Role)HttpContext.Items[CookieIdentificationMiddleware.ClientRoleKey]);
        }

        [HttpPost]
        public async Task<IActionResult> Claim(Role role)
        {
            var client = (Guid)HttpContext.Items[CookieIdentificationMiddleware.ClientIdKey];
            var currentRole = _registry.GetRole(client);
            var owner = _registry.GetOwner(role);
            var success = true;

            if (currentRole == role)
            {
                if (role != Role.Reader)
                {
                    success = TryDefend(client, role);
                }
            }
            else if (!owner.HasValue)
            {
                _registry.SetClientRole(client, role);
            }
            else
            {
                success = await TryClaimThroughDisputeAsync(client, owner.Value, role);
            }

            if (success)
            {
                return Ok();
            }
            else
            {
                return Conflict("The role is occupied.");
            }
        }

        private async Task<bool> TryClaimThroughDisputeAsync(Guid claimant, Guid defendant, Role role)
        {
            var currentDispute = _disputeFactory.GetLast(role);
            if (currentDispute == null || currentDispute.IsResolved)
            {
                var dispute = _disputeFactory.Create(claimant, defendant, role);
                return await dispute.TryClaimAsync();
            }
            return false;
        }

        private bool TryDefend(Guid client, Role role)
        {
            var currentDispute = _disputeFactory.GetLast(role);
            if (currentDispute != null && currentDispute.Defendant == client)
            {
                return currentDispute.TryDefend();
            }
            return true;
        }
    }
}
