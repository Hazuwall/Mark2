using Common;

namespace Plugins.Motion.Contracts
{
    public static class MotionMessageHeaders
    {
        private const string Query = MessageHeaders.Query;
        private const string Command = MessageHeaders.Command;
        private const string Event = MessageHeaders.Event;

        [MessageDeclarationCollection]
        public static class Queries
        {
            [MessageDeclaration(Out = typeof(Vector6))]
            public const string Coords = Query + "Coords";

            [MessageDeclaration(Out = typeof(Vector6))]
            public const string AbsCoords = Query + "AbsCoords";

            [MessageDeclaration(Out = typeof(Vector6))]
            public const string Velocities = Query + "Velocities";

            [MessageDeclaration(Out = typeof(Vector6))]
            public const string AbsVelocities = Query + "AbsVelocities";
        }

        [MessageDeclarationCollection]
        public static class Commands
        {
            [MessageDeclaration(Level = AccessLevel.Admin)]
            public const string Freeze = Command + "Freeze";
        }

        [MessageDeclarationCollection]
        public static class Events
        {
        }
    }
}
