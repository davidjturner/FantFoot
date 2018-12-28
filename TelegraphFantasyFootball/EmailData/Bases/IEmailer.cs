using System.Net.Mail;

namespace TelegraphFantasyFootball.EmailData.Bases
{
    public interface IEmailer
    {
        void Send(MailMessage email);

        void Send(Email email);
    }
}
