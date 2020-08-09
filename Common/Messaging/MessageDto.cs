using Newtonsoft.Json.Linq;

namespace Common.Messaging
{
    public class MessageDto
    {
        public string MessageHeader { get; set; }
        public JObject Payload { get; set; }
    }
}
