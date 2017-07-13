using System.Linq;
using System.Threading.Tasks;
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

        public override bool IsAvailable(Hero hero)
        {
            if (!base.IsAvailable(hero)) return false;
            if (hero.GetInventory().Any(item => item is HolyRelic)) return false;
            var locationInventory = hero.GetLocationInventory().ToList();
            var hasKeys = locationInventory.Count(item => item is Key) >= 3;
            return hasKeys;
        }

        public override Task ExecuteAsync(Hero hero)
        {
            if (!IsAvailable(hero))
                throw new CommandNotAvailableException(hero, this);
            var relic = hero.Game.CreateItem("Holy Relic");
            hero.AddToInventory(relic);
            var keys = hero.GetLocationInventory().Where(x => x is Key).Take(3).ToList();
            foreach (var key in keys)
                key.Owner.RemoveFromInventory(key);
            var space = hero.Space;
            space.HasRelic = false;
            space.RemoveAction(Name);
            return Task.CompletedTask;
        }

    }
}
