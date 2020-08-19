using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public class OperationContract
    {
        public Type ParameterType { get; set; } = typeof(void);
        public Type ReturnType { get; set; } = typeof(void);
        public Role Role { get; set; } = Role.Reader;
        public string Description { get; set; }

        public override string ToString()
        {
            return $"Parameter: {ParameterType}, Return: {ReturnType}, Role: {Role}";
        }
    }
}
