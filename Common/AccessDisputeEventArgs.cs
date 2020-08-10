using System;

namespace Common
{
    public class AccessDisputeEventArgs : EventArgs
    {
        public AccessLevel Level { get; set; }
        public AccessDisputeStatus Status { get; set; }

        public override string ToString()
        {
            return $"Level: {Level}, Status: {Status}";
        }
    }
}
