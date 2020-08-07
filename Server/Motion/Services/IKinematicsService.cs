using Common.Entities;

namespace Server.Motion.Services
{
    public interface IKinematicsService
    {
        Vector6 ForwardPosTransform(Vector6 coords);
        Vector6 ReversePosTransform(Vector6 absCoords);
        Vector3 ForwardVelocityTransform(Vector6 velocities);
        Vector6 ReverseVelocityTransform(Vector3 absVelocities);
    }
}
