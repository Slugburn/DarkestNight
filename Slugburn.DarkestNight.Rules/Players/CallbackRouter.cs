﻿using System;
using System.Text.RegularExpressions;
using Slugburn.DarkestNight.Rules.Events;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Players
{
    public class CallbackRouter
    {
        private static readonly Regex _regex = new Regex(@"^(?<type>[A-Za-z]*):(?<name>[A-Za-z\W]*)(?:\/(?<path>.*))?$");

        public static void Route(Game game, Callback callback, object data)
        {
            var hero = game.GetHero(callback.HeroName);
            if (callback.Handler == null)
            {
                var route = GetRoute(hero, callback);
                var handler = route.Item1;
                var path = route.Item2;
                handler.HandleCallback(hero, path, data);
            }
            else
            {
                callback.Handler.HandleCallback(hero, null, data);
            }
        }

        private static Tuple<ICallbackHandler, string> GetRoute(Hero hero, Callback callback)
        {
            var route = callback.Route;
            var match = _regex.Match(route);
            if (!match.Success)
                throw new ArgumentOutOfRangeException(nameof(callback.Route), callback.Route);
            var type = match.Groups["type"].Value;
            var name = match.Groups["name"].Value;
            var path = match.Groups["path"].Value;
            var handler = GetHandler(hero, type, name);
            return Tuple.Create(handler, path);
        }

        private static ICallbackHandler GetHandler(Hero hero, string callbackType, string name)
        {
            switch (callbackType)
            {
                case "Action":
                    return (ICallbackHandler) hero.GetCommand(name);
                case "Event":
                    return (ICallbackHandler) EventFactory.CreateCard(name);
                case "Power":
                    return (ICallbackHandler) hero.GetPower(name);
                case "Type":
                    var type = typeof(CallbackRouter).Assembly.GetType(name, true);
                    return (ICallbackHandler) Activator.CreateInstance(type);
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), callbackType);
            }
        }
    }

}
