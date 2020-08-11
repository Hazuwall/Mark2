using System;

namespace Common
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class MessageInfoAttribute : Attribute
    {
        public Type In { get; set; } = typeof(void);
        public Type Out { get; set; } = typeof(void);
        public Role Role { get; set; } = Role.Reader;

        public override string ToString()
        {
            return $"Input: {In}, Output: {Out}, Role: {Role}";
        }
    }
}