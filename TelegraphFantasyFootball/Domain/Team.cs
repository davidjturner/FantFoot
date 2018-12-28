using System.Collections.Generic;

namespace TelegraphFantasyFootball.Domain
{
    public class Team
    {
        public string Name { get; set; }

        public List<Player> Players { get; set; }
    }
}
