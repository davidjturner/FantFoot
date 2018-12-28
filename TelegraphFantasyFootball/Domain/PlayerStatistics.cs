using System.Collections.Generic;
using System.Text;
using TelegraphFantasyFootball.Domain.enums;

namespace TelegraphFantasyFootball.Domain
{
    public class PlayerStatistics
    {
        public Player Player { get; set; }

        public List<Statistics> Statistics = new List<Statistics>();

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine("-------------------------------------------------------");
            sb.AppendLine("-------------------------------------------------------");
            sb.AppendLine();

            sb.AppendFormat("Player: {0} {1}", Player.Initial, Player.LastName);
            sb.AppendLine();

            foreach (var statistic in Statistics)
            {
                sb.AppendFormat("Website: {0}", statistic.Source);
                sb.AppendLine();

                if (statistic.IsPlaying != IsPlaying.NotApplicable)
                {
                    sb.AppendFormat("Is Playing: {0}", statistic.IsPlaying);
                }

                sb.AppendLine();

                if (statistic.Injured != Injured.NotApplicable)
                {
                    sb.AppendFormat("Injured: {0}", statistic.Injured);
                }

                sb.AppendLine();

                if (statistic.InjuryType != Identifiers.NotApplicable.ToString())
                {
                    sb.AppendFormat("InjuryType: {0}", statistic.InjuryType);
                }

                sb.AppendLine();

                if (statistic.ReturnDate != Identifiers.NotApplicable.ToString())
                {
                    sb.AppendFormat("ReturnDate: {0}", statistic.ReturnDate);
                }

                sb.AppendLine();

                if (statistic.IsFitForNextMatch != Identifiers.NotApplicable.ToString())
                {
                    sb.AppendFormat("Will be fit by next match: {0}", statistic.IsFitForNextMatch);
                }

                sb.AppendLine();
                sb.AppendLine();
            }

            sb.AppendLine("-------------------------------------------------------");
            sb.AppendLine("-------------------------------------------------------");
            
            return sb.ToString();
        }
    }
}
