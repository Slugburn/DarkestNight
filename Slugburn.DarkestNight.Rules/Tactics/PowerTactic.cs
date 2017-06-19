using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Powers;

namespace Slugburn.DarkestNight.Rules.Tactics
{
    internal class PowerTactic : ITactic
    {
        public string PowerName { get; set; }
        public int DiceCount { get; set; }

        public virtual string Name => $"{PowerName}";
        public TacticType Type { get; set; } 
        public virtual void Use(Hero hero) { }

        public bool IsAvailable(Hero hero) => hero.GetPower(PowerName).IsUsable(hero);
        public int GetDiceCount() => DiceCount;

    }
}