using System.Collections.Generic;
using System.Net.Mail;

namespace TelegraphFantasyFootball.EmailData.Bases
{
    public abstract class Email
    {
        protected Email(MailAddress to, MailAddress from, string firstName = null, string cultureInfo = null, IList<MailAddress> cc = null)
        {
            To = to;
            From = from;
            Cc = cc;
            FirstName = firstName;
            CultureInfo = cultureInfo;
        }

        public MailAddress To { get; private set; }
        public MailAddress From { get; private set; }
        public IList<MailAddress> Cc { get; private set; }

        public string FirstName { get; private set; }
        public string CultureInfo { get; private set; }

        public string EmailBody { get; set; }

        public virtual bool IsBodyHtml
        {
            get
            {
                return false;
                
            }
        }

        public abstract string Subject { get; }
        
    }
}
