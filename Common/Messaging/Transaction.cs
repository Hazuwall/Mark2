using System;
using System.Collections.Generic;
using System.Net;

namespace Common.Messaging
{
    public class Transaction
    {
        public Transaction(Guid id, Guid recieverId, EndPoint endPoint, int protocolVersion, string[] flags,
                           Message operation)
        {
            Id = id;
            RecieverId = recieverId;
            EndPoint = endPoint;
            ProtocolVersion = protocolVersion;
            Flags = flags;
            Operation = operation;
        }

        public Guid Id { get; }
        public Guid RecieverId { get; }
        public EndPoint EndPoint { get; }
        public int ProtocolVersion { get; }
        public string[] Flags { get; }
        public Message Operation { get; set; }
        public Message Result { get; set; }

        public override string ToString()
        {
            return $"Operation: {Operation}, Id: {Id}";
        }
    }
}
