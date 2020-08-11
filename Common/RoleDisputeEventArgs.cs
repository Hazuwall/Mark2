using System;

namespace Common
{
    public class RoleDisputeEventArgs : EventArgs
    {
        public Role Role { get; set; }
        public RoleDisputeStatus Status { get; set; }

        public override string ToString()
        {
            return $"Role: {Role}, Status: {Status}";
        }
    }
}
