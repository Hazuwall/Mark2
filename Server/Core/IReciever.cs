using Common.Messaging;
using System;
using System.Threading.Tasks.Dataflow;

namespace Server.Core
{
    public interface IReciever
    {
        public Guid Id { get; }
        public ITargetBlock<Transaction> TargetBlock { get; }
    }
}
