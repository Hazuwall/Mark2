using Common;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server
{
    public class MessageDeclarationRegistry : IMessageDeclarationRegistry
    {
        public Dictionary<string, MessageDeclarationAttribute> Declarations { get; }

        public MessageDeclarationRegistry()
        {
            Declarations =
                (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                 from type in assembly.GetTypes()
                 let typeAttributes = type.GetCustomAttributes(typeof(MessageDeclarationCollectionAttribute), false)
                 where typeAttributes != null && typeAttributes.Length > 0
                 from field in type.GetFields()
                 let fieldAttributes = field.GetCustomAttributes(typeof(MessageDeclarationAttribute), false)
                 where fieldAttributes != null && fieldAttributes.Length > 0
                 select KeyValuePair.Create((string)field.GetValue(null), (MessageDeclarationAttribute)fieldAttributes[0]))
                .ToDictionary(t => t.Key, t => t.Value);

            if (Log.IsEnabled(Serilog.Events.LogEventLevel.Information))
            {
                Log.Information("Registered messages: {0}", string.Join(", ", Declarations.Keys));
            }
        }
    }
}
