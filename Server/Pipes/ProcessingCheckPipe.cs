using Common;
using System;

namespace Server.Pipes
{
    public class ProcessingCheckPipe : IPipe
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
                    throw new InvalidOperationException("The operation is not processed.");
                }
            }
        }
    }
}
