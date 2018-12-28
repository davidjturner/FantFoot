using System.Collections.Generic;
using System.Net.Mail;
using TelegraphFantasyFootball.EmailData.Bases;

namespace TelegraphFantasyFootball.EmailData
{
    public class TelegraphPlayerResultsEmail : Email
    {
        public TelegraphPlayerResultsEmail(MailAddress to, MailAddress @from, string firstName = null, string cultureInfo = null, IList<MailAddress> cc = null) : base(to, @from, firstName, cultureInfo, cc)
        {
        }

        public override string Subject
        {
            get { return "Telegraph Fanatasy Football Player News"; }
        }
    }
}
