using Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Pipes
{
    public class HelpPipe : IPipe
    {
        private readonly IMessageDeclarationRegistry _registry;

        public HelpPipe(IMessageDeclarationRegistry registry)
        {
            _registry = registry;
        }

        public void Process(Transaction transaction)
        {
            if(transaction.Operation.Header == MessageHeaders.Queries.Help)
            {
                transaction.Result = new Message(MessageHeaders.Queries.Help, _registry.Declarations);
            }
        }
    }
}
