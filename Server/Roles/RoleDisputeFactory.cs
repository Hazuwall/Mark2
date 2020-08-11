using Common;
using Common.Services;
using System;

namespace Server.Roles
{
    public class RoleDisputeFactory : IRoleDisputeFactory
    {
        private readonly IEventRaiser _raiser;
        private readonly IClientRoleRegistry _registry;

        public RoleDisputeFactory(IEventRaiser raiser, IClientRoleRegistry registry)
        {
            _raiser = raiser;
            _registry = registry;
        }

        public IRoleDispute Create(Guid claimant, Guid defendant, Role role)
        {
            return new RoleDispute(claimant, defendant, role, _registry, _raiser);
        }
    }
}
