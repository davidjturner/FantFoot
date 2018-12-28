using System.Net.Mail;

namespace TelegraphFantasyFootball.EmailData.Bases
{
    public interface IMailClient
    {
        void Send(MailMessage message);
    }
}
