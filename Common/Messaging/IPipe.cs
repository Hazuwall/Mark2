using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Messaging
{
    public interface IPipe
    {
        void Process(Transaction transaction);
    }
}
