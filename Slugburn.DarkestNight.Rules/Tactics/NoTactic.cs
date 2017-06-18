using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Tactics
{
    public class NoTactic : ITactic
    {
        public string Name => "None";

        public void Use(Hero hero)
        {
            // no action
        }

        public bool IsAvailable(Hero hero) => true;

        public int GetDiceCount() => 1;
    }
}
