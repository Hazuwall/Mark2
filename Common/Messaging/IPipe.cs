using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Messaging
{
    public interface IPipe
    {
        Transaction Process(Transaction transaction);
    }
}
