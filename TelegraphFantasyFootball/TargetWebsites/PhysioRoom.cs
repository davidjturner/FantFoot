using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using HtmlAgilityPack;
using TelegraphFantasyFootball.Domain;
using TelegraphFantasyFootball.Domain.enums;

namespace TelegraphFantasyFootball.TargetWebsites
{
    public class PhysioRoom
    {
        private Uri PhysioRoomUrl = new Uri("http://www.physioroom.com/news/english_premier_league/epl_injury_table.php");

        public void AddStatistics(List<PlayerStatistics> playersStatistics)
        {
            var htmlDocument = GetHtmlDocument();

            var table = htmlDocument.DocumentNode.Descendants("div").Where
                (x => (x.Attributes["class"] != null &&
                x.Attributes["class"].Value.Equals("injury-table"))).ToList().FirstOrDefault();

            var allPlayerStatisticsRows = table.Descendants("tr");

            // First setup players statistics with a default set of Statistics for this website
            // and update if we find any data
            foreach (var playerStatistics in playersStatistics)
            {
                playerStatistics.Statistics.Add(new Statistics
                {
                    Source = Source.Physioroom,
                    Injured = Injured.No,
                    InjuryType = Identifiers.NotApplicable.ToString(),
                    ReturnDate = Identifiers.NotApplicable.ToString(),
                    IsFitForNextMatch = Identifiers.NotApplicable.ToString(),
                    IsPlaying = IsPlaying.NotApplicable,                   
                });
            }

            foreach (var playerStatisticsRow in allPlayerStatisticsRows)
            {
                var playerStatisticsDataElements = playerStatisticsRow.Descendants("td").ToList();

                if (playerStatisticsDataElements.Count < 5)
                {
                    // This means its not a player
                    continue;
                }

                var playerNameSplit = playerStatisticsDataElements[0].InnerText.Trim().Split(new[] { ' ' }, 2);

                var initial = playerNameSplit.Count() == 1 ? "No initial" : playerNameSplit[0];
                var lastName = playerNameSplit.Count() == 1 ? playerNameSplit[0] : playerNameSplit[1];

                var player = new Player(lastName, initial);

                // Try and see if the player found in physio room matches one of our players
                // If so update it with the stats found 
                var playerStatistic = playersStatistics.FirstOrDefault(x => x.Player.Equals(player));

                // If the player found in physio room is not a player we care about, continue 
                if (playerStatistic == null)
                {
                    continue;
                }

                // Injury Type
                var injuryType = playerStatisticsDataElements[1].InnerText;

                // retuyrn date
                var returnDate = playerStatisticsDataElements[3].InnerText;

                // Next Match likelihood
                var isFitForNextMatch = playerStatisticsDataElements[4].InnerText;

                // Retrieve the default statistic that we added at the start of this method and update it with the values found
                var statistic = playerStatistic.Statistics.Last();

                statistic.Injured = Injured.Yes;
                statistic.InjuryType = injuryType;
                statistic.ReturnDate = returnDate;
                statistic.IsFitForNextMatch = isFitForNextMatch;
            }
        }

        public HtmlDocument GetHtmlDocument()
        {
            var webHelper = new WebHelper();
            var teamRequest = webHelper.GetWebRequest(PhysioRoomUrl.ToString(), "GET", null, PhysioRoomUrl.Host);

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
    }
}
