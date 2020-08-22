using Common;
using NJsonSchema;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Contracts
{
    public class ContractRegistry : IContractRegistry
    {
        private readonly IContractFactory _factory;

        public ContractRegistry(IContractFactory factory)
        {
            _factory = factory;
        }

        public Dictionary<string, OperationContract> OperationContracts { get; } = new Dictionary<string, OperationContract>();
        public Dictionary<string, string> DataContracts { get; } = new Dictionary<string, string>();
        public Dictionary<string, EventContract> EventContracts { get; } = new Dictionary<string, EventContract>();

        public bool TryGetOperationContract(string title, out OperationContract contract)
        {
            return OperationContracts.TryGetValue(title, out contract);
        }

        public bool TryGetEventContract(string title, out EventContract contract)
        {
            return EventContracts.TryGetValue(title, out contract);
        }

        public bool TryGetDataContract(string typeName, out string contract)
        {
            return DataContracts.TryGetValue(typeName, out contract);
        }

        public void RegisterServiceContract<T>() where T : class
        {
            var type = typeof(T);
            foreach (var method in type.GetMethods())
            {
                if (_factory.TryCreateOperationContract(method, out OperationContract operation))
                {
                    if (!OperationContracts.TryAdd(method.Name, operation))
                    {
                        if (Log.IsEnabled(Serilog.Events.LogEventLevel.Warning))
                        {
                            Log.Warning("The operation {0} occurs multiple times.", method.Name);
                        }
                    }
                    RegisterDataContract(operation.ParameterType);
                    RegisterDataContract(operation.ReturnType);
                }
            }
            foreach (var evnt in type.GetEvents())
            {
                if (_factory.TryCreateEventContract(evnt, out EventContract contract))
                {
                    if (!EventContracts.TryAdd(evnt.Name, contract))
                    {
                        if (Log.IsEnabled(Serilog.Events.LogEventLevel.Warning))
                        {
                            Log.Warning("The event {0} occurs multiple times.", evnt.Name);
                        }
                    }
                    RegisterDataContract(contract.ParameterType);
                }
            }
        }

        private void RegisterDataContract(Type type)
        {
            var schema = JsonSchema.FromType(type).ToJson();
            DataContracts.TryAdd(type.FullName, schema);
        }
    }
}
