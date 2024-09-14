namespace LibMessage
{
    public interface IMessageService
    {
        void StoreMessage(string message);
        string RetrieveMessage();
    }

    public class MessageService : IMessageService
    {
        private string _message;

        public void StoreMessage(string message)
        {
            _message = message;
        }

        public string RetrieveMessage()
        {
            return _message;
        }
    }

}
