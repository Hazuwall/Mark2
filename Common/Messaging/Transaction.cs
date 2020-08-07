using System;
using System.Collections.Generic;
using System.Net;

namespace Common.Messaging
{
    public class Transaction
    {
        public Transaction(Guid clientId, EndPoint endPoint, Guid id, int protocolVersion,
                           TransactionType transactionType, string[] flags, List<Message> messages)
        {
            ClientId = clientId;
            EndPoint = endPoint;
            Id = id;
            ProtocolVersion = protocolVersion;
            TransactionType = transactionType;
            Flags = flags;
            Messages = messages;
            Response = new Message[transactionType == TransactionType.Query ?
                messages.Count
                :
                1];
        }

        public Guid ClientId { get; }
        public EndPoint EndPoint { get; }
        public Guid Id { get; }
        public int ProtocolVersion { get; }
        public TransactionType TransactionType { get; }
        public string[] Flags { get; }
        public List<Message> Messages { get; }
        public Message[] Response { get; }

        public override string ToString()
        {
            return $"Type: {TransactionType}, Id: {Id}";
        }
    }
}
