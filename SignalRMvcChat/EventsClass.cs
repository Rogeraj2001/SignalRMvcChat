namespace HTTPChat_Application;

public class EventsClass
{
    public class MessageEventClass
    {
        protected virtual void OnMessage(messageEventArgs args) {
            Console.WriteLine(args.text + "!!!!!!!");
            EventHandler<messageEventArgs> handler = Message;
            handler?.Invoke(this, args);
        }
        public event EventHandler<messageEventArgs> Message;
    }

    public class messageEventArgs : EventArgs
    {
        public int id { get; set; }
        public string text { get; set; }
    }


}
