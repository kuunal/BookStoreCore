using EmailService;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.MQServices
{
    public interface IMqServices
    {
        void AddToQueue(Message email);
    }
}
