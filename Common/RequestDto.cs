using Newtonsoft.Json.Linq;
using System;

namespace Common
{
    public class RequestDto
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
        public string[] Flags { get; set; }
        public string Header { get; set; }
        public JToken Payload { get; set; }
    }
}
