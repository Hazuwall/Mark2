using Common.Entities;

namespace Common.Messaging
{
    public static class MessageHeaders
    {
        public const string Query = "Query/";
        public const string Command = "Command/";
        public const string Event = "Event/";
        public const string Error = "Error/";

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
