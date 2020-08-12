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
        public Dictionary<string, MessageInfoAttribute> Dictionary { get; } = new Dictionary<string, MessageInfoAttribute>();

        public MessageInfoRegistry()
        {
            foreach(var type in FindCollectionTypes())
            {
                RetrieveAndRegisterDeclarations(type);
            }

            if (Log.IsEnabled(Serilog.Events.LogEventLevel.Information))
            {
                Log.Information("Registered messages: {0}", string.Join(", ", Dictionary.Keys));
            }
        }

        private IEnumerable<Type> FindCollectionTypes()
        {
            return
                from assembly in AppDomain.CurrentDomain.GetAssemblies()
                from type in assembly.GetTypes()
                let attributes = type.GetCustomAttributes(typeof(MessageInfoCollectionAttribute), false)
                where attributes != null && attributes.Length > 0
                select type;
        }

        private void RetrieveAndRegisterDeclarations(Type type)
        {
            foreach(var field in type.GetFields())
            {
                var attributes = field.GetCustomAttributes(typeof(MessageInfoAttribute), false);
                if(attributes != null && attributes.Length > 0 && 
                    field.IsStatic && field.FieldType.Equals(typeof(string)))
                {
                    Dictionary.Add((string)field.GetValue(null), (MessageInfoAttribute)attributes[0]);
                }
            }
            
            foreach (var nestedType in type.GetNestedTypes())
            {
                RetrieveAndRegisterDeclarations(nestedType);
            }
        }
    }
}
