using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Players.Models
{
    public class PlayerHeroSelection
    {

        public ICollection<string> Heroes { get; set; }

        public PlayerHeroSelection(IEnumerable<Hero> heroes) : base()
        {
            Heroes = heroes.Select(h=>h.Name).ToList();
        }
    }
}
