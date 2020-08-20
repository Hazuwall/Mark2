using Common;
using System;
using System.Collections.Generic;

namespace Common
{
    public interface IContractRegistry
    {
        Dictionary<string, OperationContract> OperationContracts { get; }
        Dictionary<string, EventContract> EventContracts { get; }
        Dictionary<string, string> DataContracts { get; }
        bool TryGetOperationContract(string title, out OperationContract contract);
        bool TryGetEventContract(string title, out EventContract contract);
        bool TryGetDataContract(string typeName, out string contract);
        void RegisterServiceContract<T>() where T : class;
    }
}
