using System.Collections.Generic;
using System.Linq;
using TelegraphFantasyFootball.Domain;
using TelegraphFantasyFootball.Domain.enums;
using TelegraphFantasyFootball.EmailData;
using TelegraphFantasyFootball.TargetWebsites;
using TelegraphFantasyFootball.Views;

namespace TelegraphFantasyFootball
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var telegraphFantasyFootball = new TelegraphFantasyFootball();
            var players = telegraphFantasyFootball.GetPlayers();

            var playersStatistics = GetPlayerStatistics(players);

            var physioRoom = new PhysioRoom();
            physioRoom.AddStatistics(playersStatistics);

            var fantasyFootballScout = new FantasyFootballScout();
            fantasyFootballScout.AddStatistics(playersStatistics);

            var emailView = new EmailView();
            var injuredOrPlayersNotStartingEmail = emailView.GetInjuredOrPlayersNotStartingEmail(playersStatistics);

            var emailer = new Emailer();
            emailer.Send(injuredOrPlayersNotStartingEmail);
        }

        private static IList<Player> GetPlayers(IEnumerable<Player> allPlayers, TeamNames teamName)
        {
            return allPlayers.Where(x => x.TeamNames.Contains(teamName)).ToList();
        }

        /// <summary>
        /// Each target website will update this with their own Statistics object
        /// </summary>
        /// <param name="players"></param>
        /// <returns></returns>
        private static List<PlayerStatistics> GetPlayerStatistics(IEnumerable<Player> players)
        {            
            return players.Select(player => new PlayerStatistics
            {
                Player = player
            }).ToList();
        }

    }
}
