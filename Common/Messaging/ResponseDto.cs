using System;

namespace Common.Messaging
{
    public class ResponseDto
    {
        public Guid RequestId { get; set; }
        public Message Result { get; set; }
    }
}
