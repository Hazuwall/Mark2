using Common;
using Server.Roles;
using System;

namespace Server.Pipes
{
    public class RoleManagementPipe : IPipe
    {
        private readonly IRoleManager _manager;

        public RoleManagementPipe(IRoleManager manager)
        {
            _manager = manager;
        }

        public void Process(Transaction transaction)
        {
            var header = transaction.Operation.Header;
            if(header == MessageHeaders.Queries.Role)
            {
                var clientRole = _manager.GetRole(transaction.SenderId);
                transaction.Result = new Message(header, clientRole);
                transaction.Operation = null;
            }
            else if(header == MessageHeaders.Commands.ClaimRole)
            {
                var success = _manager.ClaimRole(transaction.SenderId,
                    (Role)transaction.Operation.Payload);
                transaction.Result = new Message(header, success);
                transaction.Operation = null;
            }
        }
    }
}
