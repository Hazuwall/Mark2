using Common;
using Common.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Server.Roles
{
    public sealed class RoleDispute : IRoleDispute
    {
        private readonly IEventRaiser _raiser;
        private readonly IClientRoleRegistry _registry;
        private CancellationTokenSource _cancellationSource;
        private volatile bool _isResolved = false;

        public RoleDispute(Guid claimant,
                           Guid defendant,
                           Role role,
                           IClientRoleRegistry registry,
                           IEventRaiser raiser)
        {
            Claimant = claimant;
            Defendant = defendant;
            Role = role;
            _raiser = raiser;
            _registry = registry;
        }

        public Guid Claimant { get; }
        public Guid Defendant { get; }
        public Role Role { get; }
        public bool IsResolved => _isResolved;
        public bool IsDefended { get; private set; }

        public void Open(int timeMs)
        {
            if (!_isResolved && _cancellationSource == null)
            {
                _cancellationSource = new CancellationTokenSource();
                _raiser.Raise(
                    MessageHeaders.Events.RoleDisputeEvent,
                    new RoleDisputeEventArgs
                    {
                        Status = RoleDisputeStatus.Opened,
                        Role = Role
                    },
                    Defendant, null);
                TryClaimAsync(timeMs, _cancellationSource.Token).Start();
            }
        }

        public void Defend()
        {
            if (!_isResolved && _cancellationSource != null && !_cancellationSource.IsCancellationRequested)
            {
                _cancellationSource.Cancel();
                _cancellationSource.Dispose();
                _cancellationSource = null;
                IsDefended = true;
                Resolve(Defendant, Claimant);
            }
        }

        private async Task<bool> TryClaimAsync(int timeMs, CancellationToken token)
        {
            try
            {
                await Task.Delay(timeMs, token);
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

        private void Resolve(Guid winner, Guid loser)
        {
            _isResolved = true;
            _registry.SetClientRole(winner, Role);
            _registry.SetClientRole(loser, Role.Reader);

            _raiser.Raise(
                MessageHeaders.Events.RoleDisputeEvent,
                new RoleDisputeEventArgs
                {
                    Status = RoleDisputeStatus.Won,
                    Role = Role
                },
                winner, null);
            _raiser.Raise(
                MessageHeaders.Events.RoleDisputeEvent,
                new RoleDisputeEventArgs
                {
                    Status = RoleDisputeStatus.Lost,
                    Role = Role
                },
                loser, null);
        }
    }
}
