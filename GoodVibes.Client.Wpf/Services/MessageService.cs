using GoodVibes.Client.Wpf.Services.Abstractions;

namespace GoodVibes.Client.Wpf.Services
{
    public class MessageService : IMessageService
    {
        public string GetMessage()
        {
            return "Hello from the Message Service";
        }
    }
}
