using System;

namespace Store.Client.Components
{
    public enum MessageLevel 
    {
        Normal,
        Error,
    }
    public class GlobalMessage
    {
        public string Message { get; private set; }
        public string Color { get; private set; } = "blue";
        public MessageLevel MessageLevel { get; private set; }
        public event Action OnChange;

        public void SetMessage(string message="", MessageLevel messageLevel= MessageLevel.Normal)
        {
            Message = message;
            MessageLevel = messageLevel;
            if (MessageLevel == MessageLevel.Error)
            {
                Color = "red";
                // log the error in database;
            }
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
