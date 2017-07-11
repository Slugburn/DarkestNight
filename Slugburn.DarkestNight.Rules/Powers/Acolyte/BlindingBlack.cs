using System.Linq;
using Slugburn.DarkestNight.Rules.Actions;
using Slugburn.DarkestNight.Rules.Commands;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Powers.Acolyte
{
    public class BlindingBlack : BonusPower
    {
        public BlindingBlack()
        {
            Name = "Blinding Black";
            StartingPower = true;
            Text = "Exhaust after a Necromancer movement roll to prevent him from detecting any heroes, regardless of Secrecy.";
        }

        public override bool IsUsable(Hero hero)
        {
            var necromancer = hero.Game.Necromancer;
            return base.IsUsable(hero) && 
                necromancer.IsTakingTurn 
                && necromancer.Destination != necromancer.RolledDestination;
        }

        protected override void OnLearn()
        {
            var action = new BlindingBlackAction(this);
            Owner.AddAction(action);
        }

        public class BlindingBlackAction : PowerCommand
        {
            public BlindingBlackAction(IPower power) : base(power, false)
            {
            }

            public override void Execute(Hero hero)
            {
                if (!IsAvailable(hero))
                    throw new CommandNotAvailableException(hero, this);

                var necromancer = hero.Game.Necromancer;
                necromancer.DetectedHeroes.Clear();
                necromancer.DetermineDestination();

                Power.Exhaust(hero);
            }
        }
    }
}