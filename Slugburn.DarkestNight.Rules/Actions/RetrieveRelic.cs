using System;
using System.Linq;
using Slugburn.DarkestNight.Rules.Commands;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Items;

namespace Slugburn.DarkestNight.Rules.Actions
{
    public class RetrieveRelic : StandardAction
    {
        public const string ActionName = "Retrieve a Holy Relic";

        public RetrieveRelic() : base(ActionName)
        {
        }

        
        public override void Execute(Hero hero)
        {
            if (!IsAvailable(hero))
                throw new CommandNotAvailableException(hero, this);
            var relic = ItemFactory.Create("Holy Relic");
            hero.AddToInventory(relic);
            var keys = hero.GetLocationInventory().Where(x => x is Key).Take(3).ToList();
            foreach (var key in keys)
                key.Owner.RemoveFromInventory(key);
            var space = hero.GetSpace();
            space.HasRelic = false;
            space.RemoveAction(Name);
        }

        public override bool IsAvailable(Hero hero)
        {
            if (!base.IsAvailable(hero)) return false;
            var locationInventory = hero.GetLocationInventory().ToList();
            var hasKeys = locationInventory.Count(item=>item is Key) >= 3;
            return hasKeys;
        }
    }
}
