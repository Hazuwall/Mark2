using Common;
using System;
using System.Collections.Generic;

namespace Server.Operations
{
    public interface IApiDeclaraionRegistry
    {
        Dictionary<string, MessageInfoAttribute> MessageInfos { get; }
        bool TryGetMessageInfo(string title, out MessageInfoAttribute info);
        bool TryGetJsonSchema(Type type, out string schema);
        void RegisterMessageInfo(string title, MessageInfoAttribute declaration);
        void RegisterJsonSchema(Type type);
    }
}
