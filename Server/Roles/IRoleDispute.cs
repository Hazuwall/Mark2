using Common;
using System;

namespace Server.Roles
{
    public interface IRoleDispute
    {
        bool IsResolved { get; }
        bool IsDefended { get; }
        Guid Claimant { get; }
        Guid Defendant { get; }
        Role Role { get; }
        void Open(int timeMs);
        void Defend();
    }
}
