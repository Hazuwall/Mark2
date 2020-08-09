using Plugins.Motion.Contracts;
using System;

namespace Plugins.Motion
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
