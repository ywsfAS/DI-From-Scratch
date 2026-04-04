using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DI_From_Scratch.Tests.Services.FactoryDependecy
{
    public interface IMessageService
    {
        void Send(string message);
    }

    public class EmailService : IMessageService
    {
        private readonly string _fromAddress;

        public EmailService(string fromAddress)
        {
            _fromAddress = fromAddress;
        }

        public void Send(string message)
        {
            Console.WriteLine($"Email sent from {_fromAddress}: {message}");
        }
    }
    public class NotificationService
    {
        private readonly IMessageService _messageService;

        public NotificationService(IMessageService messageService)
        {
            _messageService = messageService;
        }

        public void Notify(string message)
        {
            _messageService.Send(message);
        }
    }
}
