using System;
using System.Collections.Generic;
using System.Net;

namespace Common.Messaging
{
    public static class TransactionExtensions
    {
        public static Transaction ToTransaction(this RequestDto dto, Guid clientId, EndPoint ep)
        {
            return new Transaction(clientId, ep, dto.Id, dto.ProtocolVersion,
                dto.TransactionType, dto.Flags, dto.Request ?? new List<Message>());
        }

        public static ResponseDto ToResponse(this Transaction t)
        {
            return new ResponseDto()
            {
                Id = t.Id,
                TransactionType = t.TransactionType,
                Response = t.Response
            };
        }
    }
}
