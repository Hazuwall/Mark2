using Common;
using System;

namespace Server.Roles
{
    public interface IRoleDisputeFactory
    {
        IRoleDispute Create(Guid claimant, Guid defendant, Role role);
        IRoleDispute GetLast(Role role);
    }
}
