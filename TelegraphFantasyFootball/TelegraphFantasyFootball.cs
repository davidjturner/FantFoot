using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using HtmlAgilityPack;
using TelegraphFantasyFootball.Cookies;
using TelegraphFantasyFootball.Domain;
using TelegraphFantasyFootball.Domain.enums;

namespace TelegraphFantasyFootball
{
    public class TelegraphFantasyFootball
    {
        private const string LoginUrl = "https://fantasyfootball.telegraph.co.uk/premier-league/log-in/";
        private const string PostData = "email=<emailurlencoded>&pass=<password>&rememberme=on&Submit=Submit";

        private const string TeamUrl = "https://fantasyfootball.telegraph.co.uk/premier-league/currentxi/{0}";//currentxi/4065127
        private readonly WebHelper webHelper = new WebHelper();

        private const string teamCutomerIdentifier = "4065127";

        private Uri GetTeamUrl(TeamNames teamName)
        {
            switch (teamName)
            {
                case TeamNames.Customer:
                    return new Uri(string.Format(TeamUrl, teamCutomerIdentifier));
                default:
                    throw new NotImplementedException(string.Format("The URL for the team name '{0}' has not been implemented yet.  Please add"));
            }
        }

        public List<Player> GetPlayers()
        {
            var players = new List<Player>();

            // Login to website and get the session cookie
            var cookieContainer = GetSessionCookieContainer();

            AddPlayers(players, cookieContainer, TeamNames.Customer);
            
            return players;
        }

        private void AddPlayers(List<Player> players, CookieContainer cookieContainer, TeamNames teamName)
        {
            var htmlDocument = GetHtmlDocument(cookieContainer, GetTeamUrl(teamName));

            var teamCustomerplayers = htmlDocument.DocumentNode.Descendants().Where
                (x => (x.Attributes["class"] != null &&
                       x.Attributes["class"].Value.Contains("p-name"))).Select(x =>
                           new Player(x.InnerHtml.Split(',')[0], x.InnerHtml.Split(',').Count() == 1 ? Identifiers.InitialNotFound.ToString() : x.InnerHtml.Split(',')[1].Trim()) { TeamNames = new List<TeamNames> { teamName } }).Skip(11).ToList();


            var telegraphComparer = new TelegraphPlayerComparer();

            for (int i = teamCustomerplayers.Count - 1; i >= 0; i--)
            {
                if (players.Contains(teamCustomerplayers[i], telegraphComparer))
                {
                    var player = players.Single(x => telegraphComparer.Equals(x, teamCustomerplayers[i]));
                    player.TeamNames.Add(teamName);
                    teamCustomerplayers.Remove(teamCustomerplayers[i]);
                }
            }
           
            players.AddRange(teamCustomerplayers);
        }

        /// <summary>
        /// Downloads the HTML aof a website and loads it into HtmlDocument
        /// </summary>
        /// <param name="cookieContainer">Must contain any session cookies if required</param>
        /// <param name="url">Website Url to download</param>
        /// <returns></returns>
        private HtmlDocument GetHtmlDocument(CookieContainer cookieContainer, Uri url)
        {
            var teamRequest = webHelper.GetWebRequest(url.ToString(), "GET", cookieContainer);

            teamRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            string html;

            using (var response = teamRequest.GetResponse())
            {
                using (var stream = response.GetResponseStream())
                {
                    var textReader = new StreamReader(stream);

                    html = textReader.ReadToEnd();
                }
            }

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            return htmlDocument;
        }

        private CookieContainer GetSessionCookieContainer()
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            // Use SecurityProtocolType.Ssl3 if needed for compatibility reasons

            var cookieContainer = new CookieContainer();
            
            var loginRequest = webHelper.GetWebRequest(LoginUrl, "POST");

            var postDataInBytes = Encoding.ASCII.GetBytes(PostData);

            // Set the content length of the string being posted.
            loginRequest.ContentLength = postDataInBytes.Length;

            // You must initialise the cookie container if you want the cookies in the headers to be added
            loginRequest.CookieContainer = cookieContainer;

            using (var requestStream = loginRequest.GetRequestStream())
            {
                requestStream.Write(postDataInBytes, 0, postDataInBytes.Length);

                var response = (HttpWebResponse)loginRequest.GetResponse();

                cookieContainer.Add(response.Cookies);          
            }

            return cookieContainer;
        } 
    }
}
