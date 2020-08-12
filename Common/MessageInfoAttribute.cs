using System;

namespace Common
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class MessageInfoAttribute : Attribute
    {
        public Type In { get; set; } = typeof(void);
        public Type Out { get; set; } = typeof(void);
        public Role Role { get; set; } = Role.Reader;
        public string Summary { get; set; } = string.Empty;

        public override string ToString()
        {
            return $"Input: {In}, Output: {Out}, Role: {Role}";
        }
    }
}