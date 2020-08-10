using Common;
using Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class ClientAccessController : IClientAccessController
    {
        private readonly IClientAccessRegistry _registry;
        private readonly IClientAccessDisputeFactory _disputeFactory;
        private readonly Dictionary<AccessLevel, IClientAccessDispute> _disputes;

        public ClientAccessController(IClientAccessRegistry registry, IClientAccessDisputeFactory disputeFactory)
        {
            _registry = registry;
            _disputeFactory = disputeFactory;
            _disputes = new Dictionary<AccessLevel, IClientAccessDispute>()
            {
                { AccessLevel.Write, null },
                { AccessLevel.Admin, null }
            };
        }

        public AccessLevel GetAccessLevel(Guid clientId)
        {
            return _registry.GetAccessLevel(clientId);
        }

        public bool ClaimAccessLevel(Guid client, AccessLevel requestedLevel)
        {
            var currentLevel = _registry.GetAccessLevel(client);
            var owner = _registry.GetOwner(requestedLevel);
            if (currentLevel == requestedLevel)
            {
                return TryDefend(client, requestedLevel);
            }
            else if (!owner.HasValue)
            {
                _registry.SetClientAccessLevel(requestedLevel, client);
                return true;
            }
            else
            {
                TryOpenDispute(client, owner.Value, requestedLevel);
                return false;
            }
        }

        private bool TryOpenDispute(Guid claimant, Guid defendant, AccessLevel level)
        {
            var currentDispute = _disputes[level];
            if(currentDispute == null || currentDispute.IsResolved)
            {
                var dispute = _disputeFactory.Create(claimant, defendant, level);
                dispute.Open(2000);
                _disputes[level] = dispute;
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool TryDefend(Guid client, AccessLevel level)
        {
            var currentDispute = _disputes[level];
            if (currentDispute != null && currentDispute.Defendant == client)
            {
                _disputes[level] = null;
                currentDispute.Defend();
                return currentDispute.IsResolved && currentDispute.IsDefended;
            }
            return true;
        }
    }
}
