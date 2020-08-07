using System;

namespace Common.Messaging
{
    public static class MessageTypes
    {
        public const string Query = "Query/";
        public const string Command = "Command/";
        public const string Event = "Event/";
        public const string Info = "Info/";
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
            public const string SetCoords = Command + "SetCoords";
            public const string SetAbsCoords = Command + "SetAbsCoords";

            [MessageDeclaration]
            public const string Freeze = Command + "Freeze";

            [MessageDeclaration]
            public const string NotifyQuequePointPassed = Command + "NotifyQuequePointPassed";
        }

        [MessageDeclarationCollection]
        public static class Events
        {
            [MessageDeclaration]
            public const string QuequePointPassed = Event + "QuequePointPassed";
        }

        [MessageDeclarationCollection]
        public static class Infos
        {
            [MessageDeclaration]
            public const string Ok = Info + "Ok";
        }

        [MessageDeclarationCollection]
        public static class Errors
        {
            [MessageDeclaration(Out = typeof(string))]
            public const string AccessDenied = Error + "AccessDenied";

            [MessageDeclaration(Out = typeof(string))]
            public const string Calculation = Error + "Calculation";
        }
    }
}
