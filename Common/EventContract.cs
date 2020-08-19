using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public class EventContract
    {
        public Type ParameterType { get; set; } = typeof(EventArgs);
        public string Description { get; set; }

        public override string ToString()
        {
            return $"Parameter: {ParameterType}";
        }
    }
}
