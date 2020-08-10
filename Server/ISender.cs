using Common;
using System;
using System.Threading.Tasks.Dataflow;

namespace Server
{
    public interface ISender
    {
        public ISourceBlock<Transaction> SourceBlock { get; }
    }
}
