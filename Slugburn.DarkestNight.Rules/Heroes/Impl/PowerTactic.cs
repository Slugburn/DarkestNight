using Slugburn.DarkestNight.Rules.Tactics;

namespace Slugburn.DarkestNight.Rules.Heroes.Impl
{
    internal class PowerTactic : ITactic
    {
        public string PowerName { get; set; }
        public int DiceCount { get; set; }

        public virtual string Name => $"{PowerName}";
        public virtual void Use(Hero hero) { }

        public bool IsAvailable(Hero hero) => hero.GetPower(PowerName).IsUsable();
        public int GetDiceCount() => DiceCount;

    }
}