using Slugburn.DarkestNight.Rules.Actions;
using Slugburn.DarkestNight.Rules.Commands;
using Slugburn.DarkestNight.Rules.Events;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Powers;

namespace Slugburn.DarkestNight.Rules.Players
{
    public class Callback
    {
        public static Callback ForAction<T>(Hero hero, T action, string path = null) where T:IAction, ICallbackHandler
        {
            path = path != null ? "/" + path : null;
            return new Callback(hero.Name, $"Action:{action.Name}{path}");
        }

        public static Callback ForPower<T>(Hero hero, T power, string path = null) where T: IPower, ICallbackHandler
        {
            path = path != null ? "/" + path : null;
            return new Callback(hero.Name, $"Power:{power.Name}{path}");
        }

        public static Callback ForEvent<T>(Hero hero, T eventCard) where T:IEventCard, ICallbackHandler
        {
            return new Callback(hero.Name, $"Event:{eventCard.Detail.Name}");
        }

        public static Callback For(Hero hero, ICallbackHandler handler)
        {
            return new Callback(hero.Name, handler);
        }

        private Callback(string heroName, string route)
        {
            HeroName = heroName;
            Route = route;
        }

        private Callback(string heroName, ICallbackHandler handler)
        {
            HeroName = heroName;
            Handler = handler;
        }

        public string HeroName { get; set; }
        public string Route { get; set; }
        public ICallbackHandler Handler { get; set; }

    }
}
