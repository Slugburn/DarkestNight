using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Rolls;
using Slugburn.DarkestNight.Rules.Tactics;

namespace Slugburn.DarkestNight.Rules.Powers.Druid
{
    class Vines : TacticPower
    {
        public Vines()
        {
            Name = "Vines";
            Text = "Exhaust to fight or elude with 4 dice.";
        }

        protected override void OnLearn()
        {
            Owner.AddTactic(new VinesTactic(this, TacticType.Elude));
            Owner.AddTactic(new VinesTactic(this, TacticType.Fight));
        }

        private class VinesTactic : PowerTactic
        {
            public VinesTactic(Vines power, TacticType type) : base(power, type, 4)
            {
            }

            public override string Name => $"{Power.Name} [{Type.ToString().ToLower()}]";

            public override void Use(Hero hero)
            {
                base.Use(hero);
                hero.CurrentRoll.AddRollHandler(new ExhaustPowerRollHandler { PowerName = Power.Name });
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