using System;

namespace Common
{
    public class ResponseDto
    {
        public Guid RequestId { get; set; }
        public Message Result { get; set; }
    }
}
