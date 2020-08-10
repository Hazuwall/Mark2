using Common;
using Common.Services;
using System;

namespace Server
{
    public class ClientAccessDisputeFactory : IClientAccessDisputeFactory
    {
        private readonly IEventRaiser _raiser;
        private readonly IClientAccessRegistry _registry;

        public ClientAccessDisputeFactory(IEventRaiser raiser, IClientAccessRegistry registry)
        {
            _raiser = raiser;
            _registry = registry;
        }

        public IClientAccessDispute Create(Guid claimant,Guid defendant, AccessLevel level)
        {
            return new ClientAccessDispute(claimant, defendant, level, _registry, _raiser);
        }
    }
}
