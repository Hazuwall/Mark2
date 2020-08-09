using System;
using System.Collections.Generic;

namespace Common.Messaging
{
    public class RequestDto
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
        public string[] Flags { get; set; }
        public Message Operation { get; set; }
    }
}
