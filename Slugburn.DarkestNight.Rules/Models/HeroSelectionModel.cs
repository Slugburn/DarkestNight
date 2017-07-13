using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Models
{
    public class HeroSelectionModel
    {

        public ICollection<string> Heroes { get; set; }

        public HeroSelectionModel(IEnumerable<Hero> heroes) 
        {
            Heroes = heroes.Select(h=>h.Name).ToList();
        }
    }
}
