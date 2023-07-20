using MimeKit;
using MimeKit.Text;

namespace NextVer.Domain.Email
{
    public class EmailMessage
    {
        public MailboxAddress Sender { get; set; }
        public MailboxAddress Receiver { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public TextFormat ContentType { get; set; } = TextFormat.Plain;
    }
}