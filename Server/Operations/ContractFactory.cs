using Common;
using Namotion.Reflection;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Server.Operations
{
    public class ContractFactory : IContractFactory
    {
        public bool TryCreateOperationContract(MethodInfo method, out OperationContract contract)
        {
            try
            {
                contract = new OperationContract();
                if (method.IsSpecialName)
                {
                    return false;
                }

                var parameters = method.GetParameters();
                if (parameters.Length > 1)
                {
                    if (Log.IsEnabled(Serilog.Events.LogEventLevel.Warning))
                    {
                        Log.Warning("Operations with multiple parameters are not supported: {0}.", method.Name);
                    }
                    contract = null;
                    return false;
                }
                else if (parameters.Length == 1)
                {
                    contract.ParameterType = parameters[0].ParameterType;
                }

                contract.ReturnType = method.ReturnType;

                var displayAttribute = method.GetCustomAttribute<DisplayAttribute>();
                contract.Description = displayAttribute?.Description;

                var roleAttribute = method.GetCustomAttribute<RoleAttribute>();
                if(roleAttribute != null)
                {
                    contract.Role = roleAttribute.Role;
                }
                return true;
            }
            catch
            {
                contract = null;
                return false;
            }
        }

        public bool TryCreateEventContract(EventInfo evnt, out EventContract contract)
        {

            try
            {
                contract = new EventContract();
                contract.ParameterType = evnt.EventHandlerType
                    .GetMethod("Invoke")
                    .GetParameters()[1]
                    .ParameterType;
                var displayAttribute = evnt.GetCustomAttribute<DisplayNameAttribute>();
                contract.Description = displayAttribute?.DisplayName;
                return true;
            }
            catch
            {
                contract = null;
                return false;
            }
        }
    }
}
