using Common;
using System;

namespace Server.Pipes
{
    public class AccessPipe : IPipe
    {
        private readonly IMessageDeclarationRegistry _declarationRegistry;
        private readonly IClientAccessController _accessController;

        public AccessPipe(IMessageDeclarationRegistry declarationRegistry, IClientAccessController accessController)
        {
            _declarationRegistry = declarationRegistry;
            _accessController = accessController;
        }

        public void Process(Transaction transaction)
        {
            var header = transaction.Operation.Header;
            if(_declarationRegistry.Declarations.TryGetValue(header, out MessageDeclarationAttribute declaration)
                && declaration.Level != AccessLevel.Read)
            {
                var clientLevel = _accessController.GetAccessLevel(transaction.SenderId);
                if (declaration.Level > clientLevel)
                    throw new AccessViolationException();
            }

            if(header == MessageHeaders.Queries.AccessLevel)
            {
                var clientLevel = _accessController.GetAccessLevel(transaction.SenderId);
                transaction.Result = new Message(header, clientLevel);
                transaction.Operation = null;
            }
            else if(header == MessageHeaders.Commands.ClaimAccessLevel)
            {
                _accessController.ClaimAccessLevel(transaction.SenderId,
                    (AccessLevel)transaction.Operation.Payload);
            }
        }
    }
}
