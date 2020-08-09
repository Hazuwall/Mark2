﻿using System;
using System.Net;

namespace Common.Messaging.Services
{
    public interface IOperationRaiser
    {
        void Raise(Message operation, Guid recieverId, EndPoint ep);
        void Raise(Message operation);
    }
}
