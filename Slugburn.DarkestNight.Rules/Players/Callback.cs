using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Players
{
    public static class Callback
    {
        public static Callback<T> For<T>(Hero hero, ICallbackHandler<T> handler)
        {
            return new Callback<T>(hero, handler);
        }
    }

    public class Callback<T>
    {
        private readonly ICallbackHandler<T> _handler;
        private readonly Hero _hero;

        internal Callback(Hero hero, ICallbackHandler<T> handler)
        {
            _hero = hero;
            _handler = handler;
        }

        public void Handle(T data)
        {
            _handler?.HandleCallback(_hero, data);
        }
    }
}