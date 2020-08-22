using Common;
using System;
using System.Threading.Tasks;

namespace Server.Roles
{
    public interface IRoleDispute
    {
        bool IsResolved { get; }
        bool IsDefended { get; }
        Guid Claimant { get; }
        Guid Defendant { get; }
        Role Role { get; }
        Task<bool> TryClaimAsync();
        bool TryDefend();
    }
}
