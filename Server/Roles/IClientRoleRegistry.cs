using Common;
using System;

namespace Server.Roles
{
    public interface IClientRoleRegistry
    {
        Guid? GetOwner(Role role);
        Role GetRole(Guid client);
        void SetClientRole(Guid client, Role role);
    }
}
