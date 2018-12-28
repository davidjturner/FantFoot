using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using TelegraphFantasyFootball.Domain;
using TelegraphFantasyFootball.Domain.enums;
using TelegraphFantasyFootball.EmailData;

namespace TelegraphFantasyFootball.Views
{
    public class EmailView
    {
        public TelegraphPlayerResultsEmail GetInjuredOrPlayersNotStartingEmail(IList<PlayerStatistics> playerStatistics)
        {
            var sb = new StringBuilder();

            foreach (var playerStatistic in playerStatistics)
            {
                var isPlayerInjuredOrNotStarting = false;

                foreach (var statistic in playerStatistic.Statistics)
                {                 
                    if (statistic.Source == Source.Physioroom)
                    {
                        if (statistic.Injured == Injured.Yes)
                        {
                            AppendPyhsioRoomTextForInjuredPlayer(sb, playerStatistic.Player, statistic);
                            isPlayerInjuredOrNotStarting = true;
                        }

                        continue;
                    }

                    if (statistic.Source == Source.FantastyFootballScout)
                    {
                        if (statistic.IsPlaying == IsPlaying.No)
                        {
                            AppendFantastyFootballScoutTextForPlayerNotStarting(sb, playerStatistic.Player, statistic);
                            isPlayerInjuredOrNotStarting = true;
                        }

                    }
                }

                if (isPlayerInjuredOrNotStarting)
                {
                    AppendPlayersTeamInfo(sb, playerStatistic.Player);
                }
            }

            var emailBody = sb.ToString();

            var email = new TelegraphPlayerResultsEmail(new MailAddress("david.turner1980@yahoo.co.uk"), new MailAddress("dturner8083@gmail.com"))
            {
                EmailBody = (!string.IsNullOrEmpty(emailBody)) ? emailBody : "No injuries have been reported by 'Physioroom' and 'Fantasy Football Scout' thinks all players are playing"
            };

            return email;
        }

        private void AppendPyhsioRoomTextForInjuredPlayer(StringBuilder sb, Player player, Statistics playerStatistics)
        {
            sb.AppendFormat("Player {0} {1} is injured ({2}) according to {3}.",
                player.Initial.Trim(), player.LastName.Trim(), playerStatistics.InjuryType.Trim(),
                playerStatistics.Source.ToString().Trim());
            sb.AppendLine();
            sb.AppendFormat("Expected Return date is {0}", playerStatistics.ReturnDate.Trim());
            sb.AppendLine();
            sb.AppendFormat("Liklihood of playing next match {0}", playerStatistics.IsFitForNextMatch.Trim());
            sb.AppendLine();
            sb.AppendLine();
        }

        private void AppendFantastyFootballScoutTextForPlayerNotStarting(StringBuilder sb, Player player, Statistics playerStatistics)
        {
            sb.AppendFormat("Player {0} {1} IS NOT playing according to {2}.", player.Initial, player.LastName, playerStatistics.Source);
            sb.AppendLine();
        }

        private void AppendPlayersTeamInfo(StringBuilder sb, Player player)
        {
            sb.AppendLine();
            sb.AppendLine("Current teams: ");

            foreach (var teamName in player.TeamNames)
            {
                sb.AppendFormat("{0} ", teamName);
            }

            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine("------------------------------------");
            sb.AppendLine();
        }
    }
}
