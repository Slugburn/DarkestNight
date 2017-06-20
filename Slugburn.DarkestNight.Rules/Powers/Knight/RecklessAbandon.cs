using System.Linq;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Rolls;
using Slugburn.DarkestNight.Rules.Tactics;

namespace Slugburn.DarkestNight.Rules.Powers.Knight
{
    class RecklessAbandon : TacticPower
    {
        public RecklessAbandon() : base(TacticType.Fight)
        {
            Name = "Reckless Abandon";
            Text = "Fight with 4 dice. Lose 1 Grace if you roll fewer than 2 successes.";
        }

        public override void Learn(Hero hero)
        {
            base.Learn(hero);
            hero.AddTactic(new RecklessAbandonTactic {PowerName = Name} );
        }

        internal class RecklessAbandonTactic : PowerTactic, IRollHandler
        {
            public RecklessAbandonTactic()
            {
                Type = TacticType.Fight;
                DiceCount = 4;
            }

            public override void Use(Hero hero)
            {
                base.Use(hero);
                hero.AddRollHandler(this);
            }

            public void HandleRoll(Hero hero)
            {
                var targetNumber = hero.ConflictState.SelectedTargets.Single().FightTarget;
                var successes = hero.Roll.Count(x => x >= targetNumber);
                if (successes < 2)
                    hero.LoseGrace();
                hero.RemoveRollHandler(this);
            }
        }
    }
}