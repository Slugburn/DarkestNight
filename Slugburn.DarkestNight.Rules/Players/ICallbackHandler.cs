using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Players
{
    public interface ICallbackHandler
    {
        void HandleCallback(Hero hero, object data);
    }
}
