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
    public class FantasyFootballScout
    {
        public Uri FantasyFootballScoutTeamNewsUrl = new Uri("http://www.fantasyfootballscout.co.uk/team-news/");

        public void AddStatistics(List<PlayerStatistics> playersStatistics)
        {
            var playersExpectedToStart = new List<Player>();

            var htmlDocument = GetHtmlDocument();

            var rowNodes = htmlDocument.DocumentNode.Descendants("ul").Where
                (x => (x.Attributes["class"] != null &&
                       x.Attributes["class"].Value.Contains("row-"))).ToList();


            foreach (var rowNode in rowNodes)
            {
                var playerNodes = rowNode.Descendants("li");

                foreach (var playerNode in playerNodes)
                {
                    var playerName = playerNode.GetAttributeValue("title", string.Empty).Trim();

                    // Sometimes the playerName is empty and is only stored in class="player-name"               

                    if (string.IsNullOrEmpty(playerName))
                    {
                        var playerNameNode = playerNode.Descendants().FirstOrDefault(x => (x.Attributes["class"] != null &&
                                                           x.Attributes["class"].Value.Contains("player-name")));

                        if (playerNameNode != null)
                        {
                            AddPlayers(playersExpectedToStart, GetPlayerFromClass(playerNameNode.InnerText));
                        }

                        continue;
                    }

                    AddPlayers(playersExpectedToStart, GetPlayerFromTitle(playerName));
                }
            }

            foreach (var playerStatistics in playersStatistics)
            {
                var statistics = new Statistics
                {
                    InjuryType = Identifiers.NotApplicable.ToString(),
                    IsFitForNextMatch = Identifiers.NotApplicable.ToString(),
                    ReturnDate = Identifiers.NotApplicable.ToString(),
                    Source = Source.FantastyFootballScout,
                    IsPlaying = playersExpectedToStart.Contains(playerStatistics.Player) ? IsPlaying.Yes : IsPlaying.No
                };

                // Sometimes certain players do not report a first initial.
                // If this is the case we can only copare on first name
                if (statistics.IsPlaying == IsPlaying.No && CompareOnLastNameOnly(playerStatistics.Player))
                {
                    var lastName = playerStatistics.Player.LastName;
                    var player = playersExpectedToStart.Where(
                        x => x.LastName.Equals(lastName, StringComparison.InvariantCultureIgnoreCase));
                    statistics.IsPlaying = (player.Any()) ? IsPlaying.Yes : IsPlaying.No;
                }


                playerStatistics.Statistics.Add(statistics);
            };
        }

        private bool CompareOnLastNameOnly(Player player)
        {
            return player.Equals(new Player("Fonte", "J")) ||
                   player.Equals(new Player("Alli", "D")) ||
                   player.Equals(new Player("Firmino", "R")) ||
                   player.Equals(new Player("Sanchez", "A")) ||
                   player.Equals(new Player("Costa", "D")) ||
                   player.Equals(new Player("Valencia", "A"));
        }

        private void AddPlayers(List<Player> playersExpectedToStart, Player player)
        {
            var cleanedPlayer = SwapForeignCharacters(player);
            playersExpectedToStart.Add(cleanedPlayer);
        }

        private Player SwapForeignCharacters(Player player)
        {
            return
                new Player(
                    player.LastName.Replace('ó', 'o')
                        .Replace('í', 'i')
                        .Replace('á', 'a')
                        .Replace('é', 'e')
                        .Replace('è', 'e'),
                    player.Initial.Replace('ó', 'o')
                        .Replace('í', 'i')
                        .Replace('á', 'a')
                        .Replace('é', 'e')
                        .Replace('è', 'e'));      
        }

        private Player GetPlayerFromClass(string playerName)
        {
            try
            {
                var playerNameSplit = playerName.Split(' ');

                var lastName = playerNameSplit.Count() == 1 ? playerNameSplit[0].Trim() : playerNameSplit[1].Trim();

                var initial = playerNameSplit.Count() == 1 ? Identifiers.InitialNotFound.ToString() : playerNameSplit[0].Trim().Substring(0, 1);

                return new Player(lastName, initial);
            }
            catch
            {
                return new Player("ERROR", "ERROR");
            }
        }

        private Player GetPlayerFromTitle(string playerName)
        {
            var playerNameSplit = playerName.Split('(');
            var lastName = playerNameSplit[0].Trim();
            string initial = null;

            try
            {
                initial = playerNameSplit[1].Trim().Substring(0, 1);
            }
            catch
            {
                initial = Identifiers.InitialNotFound.ToString();
            }

            return new Player(lastName, initial);
        }

        public HtmlDocument GetHtmlDocument()
        {
            var webHelper = new WebHelper();
            var teamRequest = webHelper.GetWebRequest(FantasyFootballScoutTeamNewsUrl.ToString(), "GET", null, FantasyFootballScoutTeamNewsUrl.Host);

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
