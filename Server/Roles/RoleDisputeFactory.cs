using Common;
using Common.Services;
using System;

namespace Server.Roles
{
    public class RoleDisputeFactory : IRoleDisputeFactory
    {
        private readonly IEventPublisher _raiser;
        private readonly IClientRoleRegistry _registry;

        public RoleDisputeFactory(IEventPublisher raiser, IClientRoleRegistry registry)
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
