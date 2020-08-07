using Common.Entities;

namespace Server.Motion.Services
{
    public interface IMovementValidationService
    {
        bool IsCoordValid(Vector6 coord);
    }
}
