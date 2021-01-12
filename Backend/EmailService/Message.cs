using MimeKit;
using System.Collections.Generic;
using System.Linq;

namespace EmailService
{
    public class Message
    {
        public List<string> To;
        public string Subject { get; set; }
        public string Content { get; set; }

        public Message(IEnumerable<string> to, string subject, string content)
        {
            To = new List<string>();
            To.AddRange(to);
            Subject = subject;
            Content = content; 
        }
    }
}
