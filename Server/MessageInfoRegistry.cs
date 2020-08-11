using Common;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server
{
    public class MessageInfoRegistry : IMessageInfoRegistry
    {
        public Dictionary<string, MessageInfoAttribute> Dictionary { get; }

        public MessageInfoRegistry()
        {
            Dictionary =
                (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                 from type in assembly.GetTypes()
                 let typeAttributes = type.GetCustomAttributes(typeof(MessageInfoCollectionAttribute), false)
                 where typeAttributes != null && typeAttributes.Length > 0
                 from field in type.GetFields()
                 let fieldAttributes = field.GetCustomAttributes(typeof(MessageInfoAttribute), false)
                 where fieldAttributes != null && fieldAttributes.Length > 0
                 select KeyValuePair.Create((string)field.GetValue(null), (MessageInfoAttribute)fieldAttributes[0]))
                .ToDictionary(t => t.Key, t => t.Value);

            if (Log.IsEnabled(Serilog.Events.LogEventLevel.Information))
            {
                Log.Information("Registered messages: {0}", string.Join(", ", Dictionary.Keys));
            }
        }
    }
}
