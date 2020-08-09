using System;
using System.Net;

namespace Common
{
    public static class TransactionExtensions
    {
        public static Transaction ToTransaction(this RequestDto dto, Guid recieverId, EndPoint ep)
        {
            return new Transaction(dto.Id, recieverId, ep, dto.Version,
                dto.Flags, dto.Operation);
        }

        public static ResponseDto ToResponse(this Transaction t)
        {
            return new ResponseDto()
            {
                RequestId = t.Id,
                Result = t.Result
            };
        }
    }
}
