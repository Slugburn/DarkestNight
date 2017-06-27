using System.Collections.Generic;
using System.Linq;

namespace Slugburn.DarkestNight.Rules.Players.Models
{
    public class PlayerHeroSelection
    {
        public ICollection<PlayerHero> Heroes { get; set; }

        public PlayerHeroSelection(IEnumerable<PlayerHero> heroes)
        {
            Heroes = heroes.ToList();
        }
    }
}
