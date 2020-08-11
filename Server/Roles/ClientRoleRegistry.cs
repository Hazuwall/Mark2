using Common;
using Serilog;
using System;

namespace Server.Roles
{
    public class ClientRoleRegistry : IClientRoleRegistry
    {
        private Guid? _adminClient = null;
        private Guid? _writeClient = null;
        private readonly object _lock = new object();

        public Guid? GetOwner(Role role)
        {
            lock (_lock)
            {
                return role switch
                {
                    Role.Writer => _writeClient,
                    Role.Admin => _adminClient,
                    _ => null,
                };
            }
        }

        public Role GetRole(Guid client)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            lock (_lock)
            {
                if (_writeClient == client)
                    return Role.Writer;
                else if (_adminClient == client)
                    return Role.Admin;
                else
                    return Role.Reader;
            }
        }

        public void SetClientRole(Guid client, Role role)
        {
            if (Log.IsEnabled(Serilog.Events.LogEventLevel.Information))
            {
                Log.Information("The {0} role is granted to client {1}.", role, client);
            }
            lock (_lock)
            {
                if (_writeClient.HasValue && _writeClient.Value == client)
                    _writeClient = null;
                if (_adminClient.HasValue && _adminClient.Value == client)
                    _adminClient = null;

                switch (role)
                {
                    case Role.Writer:
                        _writeClient = client;
                        break; ;
                    case Role.Admin:
                        _adminClient = client;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
