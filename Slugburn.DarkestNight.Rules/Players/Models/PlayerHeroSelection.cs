using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Players.Models
{
    public class PlayerHeroSelection
    {

        public ICollection<PlayerHero> Heroes { get; set; }

        public PlayerHeroSelection(IEnumerable<Hero> heroes) : base()
        {
            Heroes = heroes.Select(PlayerHero.FromHero).ToList();
        }
    }
}
