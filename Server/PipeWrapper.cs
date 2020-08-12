using Common;
using Serilog;
using System;
using System.Threading.Tasks.Dataflow;

namespace Server
{
    public class PipeWrapper
    {
        public TransformBlock<Transaction, Transaction> Wrap(IPipe pipe)
        {
            return new TransformBlock<Transaction, Transaction>(t =>
            {
                try
                {
                    if (t.Operation != null && t.Result == null)
                        pipe.Process(t);
                }
                catch (Exception ex)
                {
                    t.Result = new Message(MessageHeaders.Families.Error + ex.GetType().Name, ex);
                    if (Log.IsEnabled(Serilog.Events.LogEventLevel.Error))
                    {
                        Log.Error(ex, "An error occured while processing {0} operation.", t.Operation);
                    }
                    t.Operation = null;
                }
                return t;
            });
        }
    }
}
