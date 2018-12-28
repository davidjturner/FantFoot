using System;
using System.Collections.Generic;
using TelegraphFantasyFootball.Domain;

namespace TelegraphFantasyFootball
{
    public class TelegraphPlayerComparer : IEqualityComparer<Player>
    {
        public bool Equals(Player x, Player y)
        {
            return x.Initial.Equals(y.Initial, StringComparison.InvariantCultureIgnoreCase) &&
                   x.LastName.Equals(y.LastName, StringComparison.InvariantCultureIgnoreCase);
        }

        public int GetHashCode(Player obj)
        {
            return obj.Initial.GetHashCode() * 17 + obj.LastName.GetHashCode();
        }
    }
}
