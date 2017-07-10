using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Players
{
    public interface ICallbackHandler<in T>
    {
        void HandleCallback(Hero hero, T data);
    }
}
