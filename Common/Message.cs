namespace Common
{
    public class Message
    {
        public string Title { get; }
        public object Payload { get; }

        public Message(string title, object payload)
        {
            Title = title;
            Payload = payload;
        }

        public Message(string title) : this(title, null) { }

        public override string ToString()
        {
            return Title;
        }
    }
}
