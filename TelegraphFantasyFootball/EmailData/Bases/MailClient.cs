using System.Net.Mail;

namespace TelegraphFantasyFootball.EmailData.Bases
{
    public class MailClient : IMailClient
    {
        private readonly SmtpClient _smtpClient;

        private readonly object _smtpClientLock = new object();

        public MailClient()
        {
            _smtpClient = new SmtpClient();
        }

        public void Send(MailMessage message)
        {
            lock(_smtpClientLock)
            {
                _smtpClient.Send(message);
            }
            
        }
    }
}
