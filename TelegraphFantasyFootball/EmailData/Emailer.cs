using System.Linq;
using System.Net.Mail;
using TelegraphFantasyFootball.EmailData.Bases;

namespace TelegraphFantasyFootball.EmailData
{
    public class Emailer : IEmailer
    {
        public IMailClient MailClient { get; set; }

        public Emailer()
        {
            MailClient = new MailClient();
        }

        public void Send(MailMessage mailMessage)
        {
            MailClient.Send(mailMessage);
        }

        public void Send(Email email)
        {
            using (var message = new MailMessage())
            {
                message.Subject = email.Subject;
                message.Body = email.EmailBody;
                message.From = email.From;
                message.To.Add(email.To);

                if (email.Cc != null && email.Cc.Any())
                {
                    foreach (var cc in email.Cc)
                    {
                        message.CC.Add(cc);
                    }
                }

                message.IsBodyHtml = email.IsBodyHtml;

                MailClient.Send(message);
            }
        }
    }
}
