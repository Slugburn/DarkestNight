using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Players
{
    public class Callback
    {
        private readonly ICallbackHandler _handler;
        private readonly Hero _hero;

        private Callback(Hero hero, ICallbackHandler handler)
        {
            _hero = hero;
            _handler = handler;
        }

        public static Callback For(Hero hero, ICallbackHandler handler)
        {
            return new Callback(hero, handler);
        }

        public void Handle(object data)
        {
            _handler?.HandleCallback(_hero, data);
        }
    }
}