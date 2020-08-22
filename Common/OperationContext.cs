using System;
using System.Net;
using System.Threading.Tasks;

namespace Common
{
    public class OperationContext
    {
        public readonly TaskCompletionSource<object> _completionSource;

        public OperationContext(Guid id,
                                Message operation,
                                TaskCompletionSource<object> completionSource)
        {
            Id = id;
            CurrentOperation = operation;
            _completionSource = completionSource;
        }

        public Guid Id { get; }
        public Message CurrentOperation { get; private set; }
        public bool IsCompleted { get; private set; } = false;

        public void SetNextOperation(Message operation)
        {
            if (operation == null)
            {
                CompleteWithException(new ArgumentNullException(nameof(operation)));
            }
            else {
                CurrentOperation = operation;
            }
        }

        public void Complete(object result = null)
        {
            CurrentOperation = null;
            IsCompleted = true;
            _completionSource.SetResult(result);
        }

        public void CompleteWithException(Exception ex)
        {
            IsCompleted = true;
            _completionSource.SetException(ex);
        }

        public override string ToString()
        {
            return $"Operation: {CurrentOperation}, Id: {Id}";
        }
    }
}
