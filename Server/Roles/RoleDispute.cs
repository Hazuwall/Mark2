using Common;
using Common.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Server.Roles
{
    public sealed class RoleDispute : IRoleDispute
    {
        public readonly static int DefendTimeout = 2000;

        private readonly IEventPublisher _publisher;
        private readonly IClientRoleRegistry _registry;
        private CancellationTokenSource _cancellationSource;
        private volatile bool _isResolved = false;

        public RoleDispute(Guid claimant,
                           Guid defendant,
                           Role role,
                           IClientRoleRegistry registry,
                           IEventPublisher publisher)
        {
            Claimant = claimant;
            Defendant = defendant;
            Role = role;
            _publisher = publisher;
            _registry = registry;
        }

        public Guid Claimant { get; }
        public Guid Defendant { get; }
        public Role Role { get; }
        public bool IsResolved => _isResolved;
        public bool IsDefended { get; private set; }

        public bool TryDefend()
        {
            if (!_isResolved && _cancellationSource != null && !_cancellationSource.IsCancellationRequested)
            {
                _cancellationSource.Cancel();
                _cancellationSource.Dispose();
                _cancellationSource = null;
                Resolve(Defendant, Claimant);
                IsDefended = true;
                return true;
            }
            return _isResolved && IsDefended;
        }

        public async Task<bool> TryClaimAsync()
        {
            if (!_isResolved && _cancellationSource == null)
            {
                _cancellationSource = new CancellationTokenSource();
                /*_publisher.Post(
                    "DisputeEvent",
                    new RoleDisputeEventArgs
                    {
                        Status = RoleDisputeStatus.Opened,
                        Role = Role
                    },
                    Defendant, null));*/
                try
                {
                    await Task.Delay(DefendTimeout, _cancellationSource.Token);
                }
                catch
                {
                    return false;
                }
                _cancellationSource.Dispose();
                _cancellationSource = null;
                Resolve(Claimant, Defendant);
                return true;
            }
            return _isResolved && !IsDefended;
        }

        private void Resolve(Guid winner, Guid loser)
        {
            _isResolved = true;
            _registry.SetClientRole(winner, Role);
            _registry.SetClientRole(loser, Role.Reader);
        }
    }
}
