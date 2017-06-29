using System;
using System.Linq;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Items;

namespace Slugburn.DarkestNight.Rules.Actions
{
    public class RetrieveRelic : IAction
    {
        public string Name => "Retrieve a Holy Relic";
        public string Text { get; }

        public void Act(Hero hero)
        {
            throw new NotImplementedException();
        }

        public bool IsAvailable(Hero hero)
        {
            var locationInventory = hero.GetLocationInventory().ToList();
            return hero.IsTakingTurn && hero.IsActionAvailable && locationInventory.Count(item=>item is Key) >= 3;
        }
    }
}
