using Common.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Motion.Services
{
    public class KinematicsService : IKinematicsService
    {
        public Vector6 ForwardPosTransform(Vector6 coords)
        {
            throw new NotImplementedException();
        }

        public Vector3 ForwardVelocityTransform(Vector6 velocities)
        {
            throw new NotImplementedException();
        }

        public Vector6 ReversePosTransform(Vector6 absCoords)
        {
            throw new NotImplementedException();
        }

        public Vector6 ReverseVelocityTransform(Vector6 absVelocities)
        {
            throw new NotImplementedException();
        }
    }
}
