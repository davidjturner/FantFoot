using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TelegraphFantasyFootball.Cookies
{
    public abstract class TelegraphTeamCookie
    {
        public virtual Cookie Get()
        {
            return new Cookie
            {
                Name = "_FGTeam",
                Value = CookieValue,
                Expires = DateTime.MinValue,
                Path = "/",
                HttpOnly = false,
                Secure = false,
                Domain = "fantasyfootball.telegraph.co.uk"
            };
        }

        protected abstract string CookieValue { get; }
    }
}
