using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public class OperationContract
    {
        public Type InputType { get; set; } = typeof(void);
        public Type OutputType { get; set; } = typeof(void);
        public Role Role { get; set; } = Role.Reader;
        public string Description { get; set; }

        public override string ToString()
        {
            return $"Input: {InputType}, Output: {OutputType}, Role: {Role}";
        }
    }
}
