using System;
using System.Net;

namespace Common
{
    public class Transaction
    {
        public Transaction(Guid id,
                           Guid senderId,
                           Guid recieverId,
                           EndPoint endPoint,
                           int protocolVersion,
                           string[] flags,
                           Message operation)
        {
            Id = id;
            SenderId = senderId;
            RecieverId = recieverId;
            EndPoint = endPoint;
            ProtocolVersion = protocolVersion;
            Flags = flags;
            Operation = operation;
        }

        public Guid Id { get; }
        public Guid SenderId { get; }
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
