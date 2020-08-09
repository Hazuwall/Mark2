using Plugins.Motion.Contracts;

namespace Plugins.Motion
{
    public interface IMovementValidationService
    {
        bool IsCoordValid(Vector6 coord);
    }
}
