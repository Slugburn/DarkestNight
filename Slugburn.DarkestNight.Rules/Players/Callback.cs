using Slugburn.DarkestNight.Rules.Actions;
using Slugburn.DarkestNight.Rules.Events;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Players
{
    public class Callback
    {
        public static Callback ForAction(Hero hero, IAction action, string path = null)
        {
            path = path != null ? "/" + path : null;
            return new Callback(hero.Name, $"Action:{action.Name}{path}");
        }

        public static Callback ForEvent(Hero hero, IEventCard eventCard)
        {
            return new Callback(hero.Name, $"Event:{eventCard.Detail.Name}");
        }

        private Callback(string heroName, string route)
        {
            HeroName = heroName;
            Route = route;
        }

        public string HeroName { get; set; }
        public string Route { get; set; }
    }
}
