using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel.DataAnnotations;

namespace Common
{
    public class OperationDto
    {
        [Required]
        public string Title { get; set; }
        public JToken Payload { get; set; }
        public Guid Id { get; set; }
        public string[] Flags { get; set; }
    }
}
