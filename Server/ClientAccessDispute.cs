using Common;
using Common.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    public sealed class ClientAccessDispute : IClientAccessDispute
    {
        private readonly IEventRaiser _raiser;
        private readonly IClientAccessRegistry _registry;
        private CancellationTokenSource _cancellationSource;
        private volatile bool _isResolved = false;

        public ClientAccessDispute(Guid claimant,
                                   Guid defendant,
                                   AccessLevel level,
                                   IClientAccessRegistry registry,
                                   IEventRaiser raiser)
        {
            Claimant = claimant;
            Defendant = defendant;
            Level = level;
            _raiser = raiser;
            _registry = registry;
        }

        public Guid Claimant { get; }
        public Guid Defendant { get; }
        public AccessLevel Level { get; }
        public bool IsResolved => _isResolved;
        public bool IsDefended { get; private set; }

        public void Open(int timeMs)
        {
            if (!_isResolved && _cancellationSource == null)
            {
                _cancellationSource = new CancellationTokenSource();
                _raiser.Raise(
                    MessageHeaders.Events.AccessDisputeEvent,
                    new AccessDisputeEventArgs
                    {
                        Status = AccessDisputeStatus.Opened,
                        Level = Level
                    },
                    Defendant, null);
                _ = TryClaimAsync(timeMs, _cancellationSource.Token);
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
            _registry.SetClientAccessLevel(Level, winner);
            _registry.SetClientAccessLevel(AccessLevel.Read, loser);

            _raiser.Raise(
                MessageHeaders.Events.AccessDisputeEvent,
                new AccessDisputeEventArgs
                {
                    Status = AccessDisputeStatus.Won,
                    Level = Level
                },
                winner, null);
            _raiser.Raise(
                MessageHeaders.Events.AccessDisputeEvent,
                new AccessDisputeEventArgs
                {
                    Status = AccessDisputeStatus.Lost,
                    Level = Level
                },
                loser, null);
        }
    }
}
