namespace Common.Messaging
{
    public class Message
    {
        public string MessageType { get; }
        public object Payload { get; }

        public Message(string type, object payload)
        {
            MessageType = type;
            Payload = payload;
        }

        public override string ToString()
        {
            return MessageType;
        }
    }
}
