using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Powers;

namespace Slugburn.DarkestNight.Rules.Tactics
{
    public class BasicEludeTactic : ITactic
    {
        public string Name => "Elude";
        public TacticType Type => TacticType.Elude;

        public void Use(Hero hero)
        {
        }

        public bool IsAvailable(Hero hero) => true;

        public int GetDiceCount() => 1;
    }
}
