using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Rolls;
using Slugburn.DarkestNight.Rules.Tactics;

namespace Slugburn.DarkestNight.Rules.Powers.Druid
{
    class Vines : TacticPower
    {
        public Vines() : base()
        {
            Name = "Vines";
            Text = "Exhaust to fight or elude with 4 dice.";
        }

        protected override void OnLearn()
        {
            Owner.AddTactic(new VinesTactic { PowerName = Name, Type = TacticType.Elude, DiceCount = 4 });
            Owner.AddTactic(new VinesTactic { PowerName = Name, Type = TacticType.Fight, DiceCount = 4 });
        }

        private class VinesTactic : PowerTactic
        {
            public override string Name => $"{PowerName} [{Type.ToString().ToLower()}]";

            public override void Use(Hero hero)
            {
                base.Use(hero);
                hero.CurrentRoll.AddRollHandler(new ExhaustPowerRollHandler { PowerName = PowerName });
            }
        }

        private class ExhaustPowerRollHandler : IRollHandler
        {
            public string PowerName { get; set; }
            public RollState HandleRoll(Hero hero, RollState rollState)
            {
                return rollState;
            }

            public void AcceptRoll(Hero hero, RollState rollState)
            {
                hero.GetPower(PowerName).Exhaust(hero);
            }
        }
    }
}