using Common;
using System.Collections.Generic;

namespace Server
{
    public interface IMessageInfoRegistry
    {
        Dictionary<string, MessageInfoAttribute> Dictionary { get; }
    }
}
