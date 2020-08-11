using Common;
using Server.Roles;
using System;

namespace Server.Pipes
{
    public class AccessVerificationPipe : IPipe
    {
        private readonly IMessageInfoRegistry _operationRegistry;
        private readonly IRoleManager _roleManager;

        public AccessVerificationPipe(IMessageInfoRegistry declarationRegistry, IRoleManager roleManager)
        {
            _operationRegistry = declarationRegistry;
            _roleManager = roleManager;
        }

        public void Process(Transaction transaction)
        {
            var isFound = _operationRegistry.Dictionary.TryGetValue(transaction.Operation.Header,
                out MessageInfoAttribute operationInfo);
            if (isFound && operationInfo.Role > Role.Reader)
            {
                var clientRole = _roleManager.GetRole(transaction.SenderId);
                if (operationInfo.Role > clientRole)
                    throw new AccessViolationException();
            }
        }
    }
}
