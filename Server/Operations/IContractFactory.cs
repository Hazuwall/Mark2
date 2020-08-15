using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Server.Operations
{
    public interface IContractFactory
    {
        public bool TryCreateOperationContract(MethodInfo method, out OperationContract contract);
        public bool TryCreateEventContract(EventInfo evnt, out EventContract contract);
    }
}
