using System;
using System.Net;

namespace Common
{
    public class OperationContext
    {
        public OperationContext(Guid id,
                                string[] flags,
                                Message operation)
        {
            Id = id;
            Flags = flags;
            CurrentOperation = operation;
        }

        public Guid Id { get; }
        public string[] Flags { get; }
        public Message CurrentOperation { get; private set; }
        public object Result { get; private set; } = null;
        public bool IsCompleted { get; private set; } = false;

        public void SetNextOperation(Message operation)
        {
            CurrentOperation = operation;
        }

        public void Complete(object result = null)
        {
            CurrentOperation = null;
            Result = result;
            IsCompleted = true;
        }

        public override string ToString()
        {
            return $"Operation: {CurrentOperation}, Id: {Id}";
        }
    }
}
