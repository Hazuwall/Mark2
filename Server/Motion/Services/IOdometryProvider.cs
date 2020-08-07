using Common.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Motion.Services
{
    public interface IOdometryProvider
    {
        Vector6 GetCoords();
        Vector6 GetVelocities();
    }
}
