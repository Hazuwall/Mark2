using Common;
using Common.Services;
using System;
using System.Net;
using System.Threading.Tasks.Dataflow;

namespace Server
{
    public class EventPublisher : IEventPublisher
    {
        private readonly BufferBlock<OperationContext> _sourceBlock = new BufferBlock<OperationContext>();

        public Guid Id { get; } = Guid.NewGuid();
        public ISourceBlock<OperationContext> SourceBlock => _sourceBlock;

        public void Post(string eventName, EventArgs args, Guid recieverId, EndPoint ep)
        {
            /*var operation = new Message(eventName, args);
            var transaction = new Transaction(Guid.Empty, Id, recieverId, ep, 1, null, operation);
            _sourceBlock.Post(transaction);*/
        }

        public void Post(string eventName, EventArgs args)
        {
            Post(eventName, args, Guid.Empty, null);
        }
    }
}
