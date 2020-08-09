using Newtonsoft.Json.Linq;

namespace Common
{
    public class MessageDto
    {
        public string MessageHeader { get; set; }
        public JObject Payload { get; set; }
    }
}
