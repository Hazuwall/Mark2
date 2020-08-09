using Common;
using System;

namespace Server.Pipes
{
    public class ProcessingInspectorPipe : IPipe
    {
        public void Process(Transaction transaction)
        {
            if (transaction.Operation != null)
            {
                if (transaction.Operation.Header.StartsWith(MessageHeaders.Event))
                {
                    transaction.Result = transaction.Operation;
                    transaction.Operation = null;
                }
                else
                {
                    throw new InvalidOperationException("Operation is not processed.");
                }
            }
        }
    }
}
