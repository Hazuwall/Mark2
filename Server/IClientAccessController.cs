using Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    public interface IClientAccessController
    {
        AccessLevel GetAccessLevel(Guid clientId);
        bool ClaimAccessLevel(Guid clientId, AccessLevel level);
    }
}
