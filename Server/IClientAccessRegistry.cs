using Common;
using System;

namespace Server
{
    public interface IClientAccessRegistry
    {
        Guid? GetOwner(AccessLevel level);
        AccessLevel GetAccessLevel(Guid client);
        void SetClientAccessLevel(AccessLevel level, Guid client);
    }
}
