using Common;
using Serilog;
using System;

namespace Server
{
    public class ClientAccessRegistry : IClientAccessRegistry
    {
        private Guid? _adminClient = null;
        private Guid? _writeClient = null;
        private readonly object _lock = new object();

        public Guid? GetOwner(AccessLevel level)
        {
            lock (_lock)
            {
                return level switch
                {
                    AccessLevel.Write => _writeClient,
                    AccessLevel.Admin => _adminClient,
                    _ => null,
                };
            }
        }

        public AccessLevel GetAccessLevel(Guid client)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            lock (_lock)
            {
                if (_writeClient == client)
                    return AccessLevel.Write;
                else if (_adminClient == client)
                    return AccessLevel.Admin;
                else
                    return AccessLevel.Read;
            }
        }

        public void SetClientAccessLevel(AccessLevel level, Guid client)
        {
            if (Log.IsEnabled(Serilog.Events.LogEventLevel.Information))
            {
                Log.Information("An access level {0} is granted to client {1}.", level, client);
            }
            lock (_lock)
            {
                if (_writeClient.HasValue && _writeClient.Value == client)
                    _writeClient = null;
                if (_adminClient.HasValue && _adminClient.Value == client)
                    _adminClient = null;

                switch (level)
                {
                    case AccessLevel.Write:
                        _writeClient = client;
                        break; ;
                    case AccessLevel.Admin:
                        _adminClient = client;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
