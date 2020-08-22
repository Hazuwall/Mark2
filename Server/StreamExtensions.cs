using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Server
{
    public static class StreamExtensions
    {
        public static async Task<object> ReadJsonPayloadAsync(this Stream stream, Type type, JsonSerializer serializer)
        {
            using (var ms = new MemoryStream())
            {
                await stream.CopyToAsync(ms);
                if (ms.Length == 0)
                {
                    return null;
                }

                ms.Position = 0;
                try
                {
                    using var sr = new StreamReader(ms, leaveOpen: true);
                    using var jsonTextReader = new JsonTextReader(sr);
                    return serializer.Deserialize(jsonTextReader, type);
                }
                catch
                {
                    return null;
                }
            }
        }

        public static async Task WriteJsonPayloadAsync(this Stream stream, object payload, JsonSerializer serializer)
        {
            using (var ms = new MemoryStream())
            {
                using (var sw = new StreamWriter(ms, leaveOpen: true))
                {
                    using (var jsonTextWriter = new JsonTextWriter(sw))
                    {
                        serializer.Serialize(jsonTextWriter, payload);
                    }
                    ms.Position = 0;
                    await ms.CopyToAsync(stream);
                }
            }
        }
    }
}
