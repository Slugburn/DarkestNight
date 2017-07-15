using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Powers;

namespace Slugburn.DarkestNight.Rules.Tactics
{
    internal class PowerTactic : ITactic
    {
        public PowerTactic(ITacticPower power, TacticType type, int diceCount)
        {
            Power = power;
            DiceCount = diceCount;
            Type = type;
        }

        public ITacticPower Power { get; }
        public string PowerName { get; set; }
        public int DiceCount { get; }

        public virtual string Name => Power.Name;
        public TacticType Type { get;  } 
        public virtual void Use(Hero hero) { }

        public bool IsAvailable(Hero hero) => Power.IsUsable(hero);
        public int GetDiceCount() => DiceCount;

    }
}