using System;

namespace Common.Messaging
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class MessageDeclarationAttribute : Attribute
    {
        public Type In { get; set; }
        public Type Out { get; set; }
    }
}