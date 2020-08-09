using Common.Messaging;
using System;
using System.Threading.Tasks.Dataflow;

namespace Server.Core
{
    public interface ISender
    {
        public Guid Id { get; }
        public ISourceBlock<Transaction> SourceBlock { get; }
    }
}
