using System.Collections.Generic;
using Slugburn.DarkestNight.Rules.Tactics;

namespace Slugburn.DarkestNight.Rules.Heroes.Impl
{
    internal class PowerTactic : ITactic
    {
        public string PowerName { get; set; }
        public int DiceCount { get; set; }

        public virtual string Name => $"{ PowerName} ({DiceCount} dice)";
        public bool IsAvailable(Hero hero) => hero.GetPower(PowerName).IsUsable();
        public int GetDiceCount() => DiceCount;

        public virtual void AfterRoll(Hero hero, ICollection<int> roll)
        {
            // do nothing
        }
    }
}