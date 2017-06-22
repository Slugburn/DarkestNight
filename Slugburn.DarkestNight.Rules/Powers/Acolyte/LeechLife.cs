using System.Linq;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Rolls;
using Slugburn.DarkestNight.Rules.Tactics;

namespace Slugburn.DarkestNight.Rules.Powers.Acolyte
{
    class LeechLife : TacticPower
    {
        public LeechLife() : base(TacticType.Fight, 3)
        {
            Name = "Leech Life";
            Text = "Exhaust while not at the Monastery to fight with 3 dice. Gain 1 Grace (up to default) if you roll 2 successes. You may not enter the Monastery while this power is exhausted.";
        }

        public override void Learn(Hero hero)
        {
            base.Learn(hero);
            hero.Add(new PreventMovementEffect(location => location == Location.Monastery && Exhausted));
            hero.AddTactic(new LeechLifeTactic());
        }

        public override bool IsUsable(Hero hero)
        {
            return base.IsUsable(hero) && hero.Location != Location.Monastery;
        }

        private class LeechLifeTactic : PowerTactic, IRollHandler
        {
            public LeechLifeTactic()
            {
                PowerName = "Leech Life";
                Type = TacticType.Fight;
                DiceCount = 3;
            }

            public override void Use(Hero hero)
            {
                base.Use(hero);
                hero.AddRollHandler(this);
                hero.GetPower(PowerName).Exhaust(hero);
            }

            public RollState HandleRoll(Hero hero, RollState rollState)
            {
                return rollState;
            }

            public void AcceptRoll(Hero hero, RollState rollState)
            {
                hero.Roll.TargetNumber = hero.ConflictState.SelectedTargets.Select(x => x.FightTarget).Min();
                if (hero.Roll.Successes > 1)
                    hero.GainGrace(1, hero.DefaultGrace);
                hero.RemoveRollHandler(this);
            }
        }
    }
}