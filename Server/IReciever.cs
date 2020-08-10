using Common;
using System;
using System.Threading.Tasks.Dataflow;

namespace Server
{
    public interface IReciever
    {
        public ITargetBlock<Transaction> TargetBlock { get; }
    }
}
