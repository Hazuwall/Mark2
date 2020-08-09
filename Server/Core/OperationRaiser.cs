using Common.Messaging;
using Common.Messaging.Services;
using System;
using System.Net;
using System.Threading.Tasks.Dataflow;

namespace Server.Core
{
    public class OperationRaiser : ISender, IOperationRaiser
    {
        private readonly BufferBlock<Transaction> _sourceBlock = new BufferBlock<Transaction>();

        public Guid Id { get; } = Guid.NewGuid();
        public ISourceBlock<Transaction> SourceBlock => _sourceBlock;
        

        public void Raise(Message operation, Guid recieverId, EndPoint ep)
        {
            var transaction = new Transaction(Guid.Empty, recieverId, ep, 1, null, operation);
            _sourceBlock.Post(transaction);
        }

        public void Raise(Message operation)
        {
            Raise(operation, Guid.Empty, null);
        }
    }
}
