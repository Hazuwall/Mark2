using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public class EventContract
    {
        public Type ArgumentType { get; set; } = typeof(EventArgs);
        public string Description { get; set; } = string.Empty;
    }
}
