using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace Common
{
    public class RoleDisputeEventArgs : EventArgs
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public Role Role { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public RoleDisputeStatus Status { get; set; }

        public override string ToString()
        {
            return $"Role: {Role}, Status: {Status}";
        }
    }
}
