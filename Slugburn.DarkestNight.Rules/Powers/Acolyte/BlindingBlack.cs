using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Actions;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Powers.Acolyte
{
    public class BlindingBlack : Bonus
    {
        public BlindingBlack()
        {
            Name = "Blinding Black";
            StartingPower = true;
            Text = "Exhaust after a Necromancer movement roll to prevent him from detecting any heroes, regardless of Secrecy.";
        }

        public override bool IsUsable(Hero hero)
        {
            return base.IsUsable(hero) && hero.Game.Necromancer.IsActing && hero.Game.Necromancer.DetectedHeroes.Any();
        }

        public override void Learn(Hero hero)
        {
            base.Learn(hero);
            var action = new BlindingBlackAction(Name);
            hero.AddAction(action);
        }

        public class BlindingBlackAction : PowerAction
        {
            public BlindingBlackAction(string name) : base(name)
            {
            }

            public override void Act(Hero hero)
            {
                var power = hero.GetPower(Name);
                if (!IsAvailable(hero))
                    throw new PowerNotUsableException(power);

                var necromancer = hero.Game.Necromancer;
                necromancer.DetectedHeroes.Clear();
                necromancer.DetermineDestination();

                power.Exhaust(hero);
            }
        }
    }
}