using Common;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Operations
{
    public class OperationRequestDtoConverter : JsonConverter<OperationDto>
    {
        private readonly IApiDeclaraionRegistry _registry;

        public OperationRequestDtoConverter(IApiDeclaraionRegistry registry)
        {
            _registry = registry;
        }

        public override OperationDto ReadJson(JsonReader reader, Type objectType, [AllowNull] OperationDto existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);
            var dto = new OperationDto();
            serializer.Populate(jObject.CreateReader(), dto);
            if (!_registry.TryGetMessageInfo(dto.Title, out MessageInfoAttribute info))
            {
                throw new FormatException("An operation is not registered.");
            }
            var payloadToken = jObject[nameof(OperationDto.Payload)];
            return dto;
        }

        public override void WriteJson(JsonWriter writer, [AllowNull] OperationDto value, JsonSerializer serializer)
        {
            writer.WriteValue(value);
        }
    }
}
