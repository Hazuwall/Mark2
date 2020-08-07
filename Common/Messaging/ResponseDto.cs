using System;

namespace Common.Messaging
{
    public class ResponseDto
    {
        public Guid Id { get; set; }
        public TransactionType TransactionType { get; set; }
        public Message[] Response { get; set; }
    }
}
