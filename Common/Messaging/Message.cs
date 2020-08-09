namespace Common.Messaging
{
    public class Message
    {
        public string Header { get; }
        public object Payload { get; }

        public Message(string header, object payload)
        {
            Header = header;
            Payload = payload;
        }

        public Message(string header):this(header, null) { }

        public override string ToString()
        {
            return Header;
        }
    }
}
