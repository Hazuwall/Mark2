using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Server
{
    public class TypeJsonConverter : JsonConverter<Type>
    {
        public override Type ReadJson(JsonReader reader, Type objectType, [AllowNull] Type existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return Type.GetType(reader.ReadAsString());
        }

        public override void WriteJson(JsonWriter writer, [AllowNull] Type value, JsonSerializer serializer)
        {
            writer.WriteValue(value.FullName);
        }
    }
}
