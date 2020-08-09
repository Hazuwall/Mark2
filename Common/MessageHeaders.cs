using System.Collections.Generic;

namespace Common
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
            [MessageDeclaration(Out = typeof(Dictionary<string,MessageDeclarationAttribute>))]
            public const string Help = Query + "Help";
        }

        [MessageDeclarationCollection]
        public static class Commands
        {
        }

        [MessageDeclarationCollection]
        public static class Events
        {
        }
    }
}
