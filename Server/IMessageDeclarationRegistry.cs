using Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    public interface IMessageDeclarationRegistry
    {
        Dictionary<string, MessageDeclarationAttribute> Declarations { get; }
    }
}
