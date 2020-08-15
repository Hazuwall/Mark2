using Common;
using NJsonSchema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Operations
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
            foreach(var method in type.GetMethods())
            {
                if (_factory.TryCreateOperationContract(method, out OperationContract operation))
                {
                    OperationContracts.Add(method.Name, operation);
                    RegisterDataContract(operation.InputType);
                    RegisterDataContract(operation.OutputType);
                }
            }
            foreach(var evnt in type.GetEvents())
            {
                if(_factory.TryCreateEventContract(evnt, out EventContract contract))
                {
                    EventContracts.Add(evnt.Name, contract);
                    RegisterDataContract(contract.ArgumentType);
                }
            }
        }

        private void RegisterDataContract(Type type)
        {
            var schema = JsonSchema.FromType(type).ToJson();
            DataContracts.TryAdd(type.Name, schema);
        }
    }
}
