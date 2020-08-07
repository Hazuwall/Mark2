using System;
using System.Collections.Generic;

namespace Common.Messaging
{
    public class RequestDto
    {
        public Guid Id { get; set; }
        public int ProtocolVersion { get; set; }
        public TransactionType TransactionType { get; set; }
        public string[] Flags { get; set; }
        public List<Message> Request { get; set; }
    }
}
