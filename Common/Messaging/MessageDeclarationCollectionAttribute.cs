using System;

namespace Common.Messaging
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class MessageDeclarationCollectionAttribute : Attribute
    {
    }
}