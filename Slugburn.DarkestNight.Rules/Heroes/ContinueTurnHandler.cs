using Slugburn.DarkestNight.Rules.Players;

namespace Slugburn.DarkestNight.Rules.Heroes
{
    public class ContinueTurnHandler<T> : ICallbackHandler<T>
    {
        public void HandleCallback(Hero hero, T data)
        {
            hero.ContinueTurn();
        }
    }
}