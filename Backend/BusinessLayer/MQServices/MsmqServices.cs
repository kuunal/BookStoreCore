using EmailService;
using Experimental.System.Messaging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.MQServices
{
    public class MsmqServices : IMqServices
    {
        private readonly MessageQueue messageQueue;
        private IEmailSender _emailSender;

        public MsmqServices(IEmailSender emailSender)
        {
            this.messageQueue = new MessageQueue();
            this.messageQueue.Path = @".\private$\Bookstore";
            if (MessageQueue.Exists(this.messageQueue.Path))
            {
                this.messageQueue = new MessageQueue(this.messageQueue.Path);
            }
            else
            {
                this.messageQueue = MessageQueue.Create(this.messageQueue.Path);
            }
            _emailSender = emailSender;
        }

        public void AddToQueue(EmailService.Message message)
        {
            string mailInfo = JsonConvert.SerializeObject(message);
            messageQueue.Formatter = new BinaryMessageFormatter();
            messageQueue.ReceiveCompleted += MessageQueue_ReceiveCompleted;
            messageQueue.Send(mailInfo);
            messageQueue.BeginReceive();
            messageQueue.Close();
        }

        private void MessageQueue_ReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
        {
            var msg = messageQueue.EndReceive(e.AsyncResult);
            EmailService.Message message = JsonConvert.DeserializeObject<EmailService.Message>(msg.Body.ToString());
            _emailSender.SendEmail(message);
        }
    }
}
