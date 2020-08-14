using System;

namespace Common.Services
{
    public interface IEventRaiser
    {
        void Raise(string title, EventArgs args);
    }
}
