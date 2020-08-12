using Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Server.Web
{
    public class TransactionConverter
    {
        private readonly IMessageInfoRegistry _registry;

        public TransactionConverter(IMessageInfoRegistry registry)
        {
            _registry = registry;
        }

        public Transaction FromRequestDto(RequestDto dto, Guid senderId, Guid recieverId, EndPoint ep)
        {
            Message operation = null;
            if(dto.Header != null)
            {
                if(!_registry.Dictionary.TryGetValue(dto.Header, out MessageInfoAttribute info))
                {
                    throw new InvalidOperationException("The requested operation is not supported.");
                }
                if (info.In.Equals(typeof(void))){
                    operation = new Message(dto.Header);
                }
                else
                {
                    var payload = dto.Payload.ToObject(info.In);
                    operation = new Message(dto.Header, payload);
                }
            }
            return new Transaction(dto.Id, senderId, recieverId, ep, dto.Version,
                dto.Flags, operation);
        }

        public static ResponseDto ToResponseDto(Transaction t)
        {
            return new ResponseDto()
            {
                RequestId = t.Id,
                Header = t.Result?.Header,
                Payload = JToken.FromObject(t.Result?.Payload)
            };
        }
    }
}
