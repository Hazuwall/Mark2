using Common;
using System;
using System.Collections.Generic;

namespace Server.Roles
{
    public class RoleManager : IRoleManager
    {
        private readonly IClientRoleRegistry _registry;
        private readonly IRoleDisputeFactory _disputeFactory;
        private readonly Dictionary<Role, IRoleDispute> _disputes;

        public RoleManager(IClientRoleRegistry registry, IRoleDisputeFactory disputeFactory)
        {
            _registry = registry;
            _disputeFactory = disputeFactory;
            _disputes = new Dictionary<Role, IRoleDispute>()
            {
                { Role.Writer, null },
                { Role.Admin, null }
            };
        }

        public Role GetRole(Guid clientId)
        {
            return _registry.GetRole(clientId);
        }

        public bool ClaimRole(Guid client, Role requestedRole)
        {
            var currentRole = _registry.GetRole(client);
            var owner = _registry.GetOwner(requestedRole);
            if (currentRole == requestedRole)
            {
                return TryDefend(client, requestedRole);
            }
            else if (!owner.HasValue)
            {
                _registry.SetClientRole(client, requestedRole);
                return true;
            }
            else
            {
                TryOpenDispute(client, owner.Value, requestedRole);
                return false;
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
