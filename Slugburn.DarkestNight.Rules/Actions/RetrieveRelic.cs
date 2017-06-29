using System;
using System.Linq;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Items;

namespace Slugburn.DarkestNight.Rules.Actions
{
    public class RetrieveRelic : IAction
    {
        public const string ActionName = "Retrieve a Holy Relic";

        public string Name => ActionName;
        public string Text { get; }

        public void Act(Hero hero)
        {
            if (!IsAvailable(hero))
                throw new ActionNotAvailableException(hero, this);
            var relic = ItemFactory.Create("Holy Relic");
            hero.AddToInventory(relic);
            var keys = hero.GetLocationInventory().Where(x => x is Key).Take(3).ToList();
            foreach (var key in keys)
                key.Owner.RemoveFromInventory(key);
            var space = hero.GetSpace();
            space.HasRelic = false;
            space.RemoveAction(Name);
            hero.IsActionAvailable = false;
        }

        public bool IsAvailable(Hero hero)
        {
            var locationInventory = hero.GetLocationInventory().ToList();
            var hasKeys = locationInventory.Count(item=>item is Key) >= 3;
            return hero.IsTakingTurn && hero.IsActionAvailable && hasKeys;
        }
    }
}
