using Common;
using System;

namespace Server
{
    public interface IClientAccessDisputeFactory
    {
        IClientAccessDispute Create(Guid claimant, Guid defendant, AccessLevel level);
    }
}
