using Newtonsoft.Json.Linq;
using System;

namespace Common
{
    public class ResponseDto
    {
        public Guid RequestId { get; set; }
        public string Header { get; set; }
        public JToken Payload { get; set; }
    }
}
