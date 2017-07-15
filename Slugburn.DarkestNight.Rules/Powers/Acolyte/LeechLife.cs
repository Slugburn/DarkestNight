using System.Linq;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Rolls;
using Slugburn.DarkestNight.Rules.Tactics;

namespace Slugburn.DarkestNight.Rules.Powers.Acolyte
{
    class LeechLife : TacticPower
    {
        public LeechLife() : base()
        {
            Name = "Leech Life";
            Text = "Exhaust while not at the Monastery to fight with 3 dice. Gain 1 Grace (up to default) if you roll 2 successes. You may not enter the Monastery while this power is exhausted.";
        }

        protected override void OnLearn()
        {
            Owner.Add(new PreventMovementEffect(location => location == Location.Monastery && IsExhausted));
            Owner.AddTactic(new LeechLifeTactic(this));
        }

        public override bool IsUsable(Hero hero)
        {
            return base.IsUsable(hero) && hero.Location != Location.Monastery;
        }

        private class LeechLifeTactic : PowerTactic
        {
            public LeechLifeTactic(LeechLife power) :base(power, TacticType.Fight, 3) { }

            public override void Use(Hero hero)
            {
                base.Use(hero);
                hero.CurrentRoll.AddRollHandler<LeechLifeRoll>();
                Power.Exhaust(hero);
            }
        }

        private class LeechLifeRoll : IRollHandler
        {
            public RollState HandleRoll(Hero hero, RollState rollState)
            {
                return rollState;
            }

            public void AcceptRoll(Hero hero, RollState rollState)
            {
                hero.CurrentRoll.TargetNumber = hero.ConflictState.SelectedTargets.Select(x => x.TargetNumber).Min();
                if (hero.CurrentRoll.Successes > 1)
                    hero.GainGrace(1, hero.DefaultGrace);
            }
        }

    }
}