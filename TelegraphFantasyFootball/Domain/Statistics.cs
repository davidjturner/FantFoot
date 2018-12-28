using TelegraphFantasyFootball.Domain.enums;

namespace TelegraphFantasyFootball.Domain
{
    public class Statistics
    {
        public Source Source { get; set; }

        public IsPlaying IsPlaying { get; set; } 

        public string ReturnDate { get; set; }

        public Injured Injured { get; set; }

        public string InjuryType { get; set; }

        public string IsFitForNextMatch { get; set; }
    }
}
