using System;

namespace Common
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class MessageInfoAttribute : Attribute
    {
        public Type TIn { get; set; } = typeof(void);
        public Type TOut { get; set; } = typeof(void);
        public Role Role { get; set; } = Role.Reader;
        public string Summary { get; set; } = string.Empty;

        public override string ToString()
        {
            return $"Input: {TIn}, Output: {TOut}, Role: {Role}";
        }
    }
}