using Common.Messaging;
using System;
using System.Threading.Tasks.Dataflow;

namespace Server.Core
{
    public interface IClient
    {
        public Guid Id { get; }
        public ISourceBlock<Transaction> RecievedBlock { get; }
        public ITargetBlock<Transaction> ProcessedBlock { get; }
    }
}
