using Common;
using NJsonSchema;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Operations
{
    public class ReflectionApiDeclarationRegistry : IApiDeclaraionRegistry
    {
        public Dictionary<string, MessageInfoAttribute> MessageInfos { get; } = new Dictionary<string, MessageInfoAttribute>();
        public Dictionary<Type, string> JsonSchemas { get; } = new Dictionary<Type, string>();

        public ReflectionApiDeclarationRegistry()
        {
            foreach(var type in FindMessageInfoCollectionTypes())
            {
                RetrieveAndRegisterMessageInfos(type);
            }
            RegisterInputOutputJsonSchemas(MessageInfos.Values);

            if (Log.IsEnabled(Serilog.Events.LogEventLevel.Information))
            {
                Log.Information("Registered messages: {0}", string.Join(", ", MessageInfos.Keys));
            }
        }

        public bool TryGetMessageInfo(string title, out MessageInfoAttribute info)
        {
            return MessageInfos.TryGetValue(title, out info);
        }

        public bool TryGetJsonSchema(Type type, out string schema)
        {
            return JsonSchemas.TryGetValue(type, out schema);
        }

        public void RegisterMessageInfo(string title, MessageInfoAttribute info)
        {
            MessageInfos.TryAdd(title, info);
        }

        public void RegisterJsonSchema(Type type)
        {
            var schema = JsonSchema.FromType(type).ToJson();
            JsonSchemas.TryAdd(type, schema);
        }

        private IEnumerable<Type> FindMessageInfoCollectionTypes()
        {
            return
                from assembly in AppDomain.CurrentDomain.GetAssemblies()
                from type in assembly.GetTypes()
                let attributes = type.GetCustomAttributes(typeof(MessageInfoCollectionAttribute), false)
                where attributes != null && attributes.Length > 0
                select type;
        }

        private void RetrieveAndRegisterMessageInfos(Type type)
        {
            foreach(var field in type.GetFields())
            {
                var attributes = field.GetCustomAttributes(typeof(MessageInfoAttribute), false);
                if(attributes != null && attributes.Length > 0 && 
                    field.IsStatic && field.FieldType.Equals(typeof(string)))
                {
                    RegisterMessageInfo((string)field.GetValue(null), (MessageInfoAttribute)attributes[0]);
                }
            }
            
            foreach (var nestedType in type.GetNestedTypes())
            {
                RetrieveAndRegisterMessageInfos(nestedType);
            }
        }

        private void RegisterInputOutputJsonSchemas(IEnumerable<MessageInfoAttribute> messageInfos)
        {
            var registeredTypes = new HashSet<Type>();
            foreach(var info in messageInfos)
            {
                if (!registeredTypes.Contains(info.TIn))
                {
                    RegisterJsonSchema(info.TIn);
                }
                if (!registeredTypes.Contains(info.TOut))
                {
                    RegisterJsonSchema(info.TOut);
                }
            }
        }
    }
}
