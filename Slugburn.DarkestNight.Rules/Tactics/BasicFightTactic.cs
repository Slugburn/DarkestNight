using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Powers;

namespace Slugburn.DarkestNight.Rules.Tactics
{
    public class BasicFightTactic : ITactic
    {
        public string Name => "Fight";
        public TacticType Type => TacticType.Fight;

        public void Use(Hero hero)
        {
            // no action
        }

        public bool IsAvailable(Hero hero) => true;

        public int GetDiceCount() => 1;
    }
}
