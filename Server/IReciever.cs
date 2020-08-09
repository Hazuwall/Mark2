using Common;
using System;
using System.Threading.Tasks.Dataflow;

namespace Server
{
    public interface IReciever
    {
        public Guid Id { get; }
        public ITargetBlock<Transaction> TargetBlock { get; }
    }
}
