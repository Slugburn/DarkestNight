using System;
using System.Text.RegularExpressions;
using Slugburn.DarkestNight.Rules.Events;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Players
{
    public class CallbackRouter
    {
        private static readonly Regex _action = new Regex(@"^Action:(?<action>[A-Za-z\W]*)(?:\/(?<path>.*))?$");
        private static Regex _event = new Regex(@"^Event:(?<event>[A-Za-z\W]*)(?:\/(?<path>.*))?$");

        public static void Route(Game game, Callback callback, object data)
        {
            var hero = game.GetHero(callback.HeroName);
            var route = GetRoute(hero, callback);
            var handler = route.Item1;
            var path = route.Item2;
            handler.HandleCallback(hero, path, data);
        }

        private static Tuple<ICallbackHandler, string> GetRoute(Hero hero, Callback callback)
        {
            var route = callback.Route;
            var actionMatch = _action.Match(route);
            if (actionMatch.Success)
            {
                var actionName = actionMatch.Groups["action"].Value;
                var path = actionMatch.Groups["path"].Value;
                var action = hero.GetAction(actionName) as ICallbackHandler;
                return Tuple.Create(action, path);
            }
            var eventMatch = _event.Match(route);
            if (eventMatch.Success)
            {
                var eventName = eventMatch.Groups["event"].Value;
                var eventCard = EventFactory.CreateCard(eventName);
                var path = eventMatch.Groups["path"].Value;
                return Tuple.Create(eventCard as ICallbackHandler, path);
            }
            throw new NotImplementedException();
        }
    }

}
