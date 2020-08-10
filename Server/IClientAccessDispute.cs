using Common;
using System;

namespace Server
{
    public interface IClientAccessDispute
    {
        bool IsResolved { get; }
        bool IsDefended { get; }
        Guid Claimant { get; }
        Guid Defendant { get; }
        AccessLevel Level { get; }
        void Open(int timeMs);
        void Defend();
    }
}
