using Common;
using Common.Services;
using System;
using System.Collections.Generic;

namespace Server.Roles
{
    public class RoleDisputeFactory : IRoleDisputeFactory
    {
        private readonly IEventPublisher _publisher;
        private readonly IClientRoleRegistry _registry;
        private readonly Dictionary<Role, IRoleDispute> _disputes;

        public RoleDisputeFactory(IEventPublisher publisher, IClientRoleRegistry registry)
        {
            _publisher = publisher;
            _registry = registry;
            _disputes = new Dictionary<Role, IRoleDispute>()
            {
                {Role.Reader, null },
                { Role.Writer, null },
                { Role.Admin, null }
            };
        }

        public IRoleDispute Create(Guid claimant, Guid defendant, Role role)
        {
            return _disputes[role] = new RoleDispute(claimant, defendant, role, _registry, _publisher);
        }

        public IRoleDispute GetLast(Role role)
        {
            return _disputes[role];
        }
    }
}
