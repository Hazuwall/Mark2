using Common.Messaging;
using Serilog;
using System;
using System.Threading.Tasks.Dataflow;

namespace Server.Core
{
    public class PipeWrapper
    {
        public TransformBlock<Transaction,Transaction> Wrap(IPipe pipe)
        {
            return new TransformBlock<Transaction, Transaction>(t => {
                try
                {
                    if (t.Operation != null && t.Result == null)
                        pipe.Process(t);
                }
                catch(Exception ex)
                {
                    t.Result = new Message(MessageHeaders.Error + ex.GetType().Name, ex);
                    Log.Error(ex, $"An error occured while processing {t.Operation} operation.");
                    t.Operation = null;
                }
                return t;
            });
        }
    }
}
