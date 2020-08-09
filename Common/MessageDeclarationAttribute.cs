using System;

namespace Common
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class MessageDeclarationAttribute : Attribute
    {
        public Type In { get; set; } = typeof(void);
        public Type Out { get; set; } = typeof(void);
        public AccessLevel Level { get; set; } = AccessLevel.Read;

        public override string ToString()
        {
            return $"Input: {In}, Output: {Out}, AccessLevel: {Level}";
        }
    }
}