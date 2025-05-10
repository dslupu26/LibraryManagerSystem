namespace Bussiness.Common
{
    public class MessageService
    {
        public event Action OnMessageChanged;
        private string message = "working...";

        public string Message
        {
            get
            { return message; } 
            set 
            { this.message = value;
                if (OnMessageChanged != null)
                { 
                    OnMessageChanged.Invoke(); 
                }
            }
        }

        public String MessageType { get; private set; } = "info-message";
    }
}
