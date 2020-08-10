using System;
using System.Net;

namespace Common.Services
{
    public interface IEventRaiser
    {
        void Raise(string eventName, EventArgs args, Guid recieverId, EndPoint ep);
        void Raise(string eventName, EventArgs args);
    }
}
