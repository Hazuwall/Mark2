using System;

namespace Common.Services
{
    public interface IEventPublisher
    {
        void Post(string title, EventArgs args);
    }
}
