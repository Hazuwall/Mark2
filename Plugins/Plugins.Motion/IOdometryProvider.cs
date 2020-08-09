using Plugins.Motion.Contracts;

namespace Plugins.Motion
{
    public interface IOdometryProvider
    {
        Vector6 GetCoords();
        Vector6 GetVelocities();
    }
}
