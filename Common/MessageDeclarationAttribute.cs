using System;

namespace Common
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class MessageDeclarationAttribute : Attribute
    {
        public Type In { get; set; }
        public Type Out { get; set; }
        public AccessLevel Level { get; set; } = AccessLevel.Read;
    }
}