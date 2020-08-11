using Common;
using System;

namespace Server.Roles
{
    public interface IRoleManager
    {
        Role GetRole(Guid clientId);
        bool ClaimRole(Guid clientId, Role role);
    }
}
