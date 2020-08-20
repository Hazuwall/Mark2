using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class RoleAttribute : Attribute
    {
        public RoleAttribute(Role role)
        {
            Role = role;
        }

        public Role Role { get; }
    }
}
