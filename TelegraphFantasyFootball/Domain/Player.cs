using System;
using System.Collections.Generic;
using TelegraphFantasyFootball.Domain.enums;

namespace TelegraphFantasyFootball.Domain
{
    public class Player
    {
        public Player(string lastName, string initial)
        {
            LastName = lastName;
            Initial = initial;
        }


        public Statistics Statistics { get; set; }

        public List<TeamNames> TeamNames
        {
            get
            {
                return _teamNames;
            }
            set
            {
                if (_teamNames == null)
                {
                    _teamNames = value;
                }
                else
                {
                    _teamNames.AddRange(value);
                }               
            }
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Initial { get; set; }

        public override bool Equals(object obj)
        {
            var player = obj as Player;

            return LastName.Equals(player.LastName, StringComparison.InvariantCultureIgnoreCase) && Initial.Equals(player.Initial, StringComparison.InvariantCultureIgnoreCase);
        }

        private List<TeamNames> _teamNames;
    }
}
