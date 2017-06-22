using System.Linq;
using Slugburn.DarkestNight.Rules.Actions;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Rolls;
using Slugburn.DarkestNight.Rules.Tactics;

namespace Slugburn.DarkestNight.Rules.Powers.Acolyte
{
    class CallToDeath : ActionPower
    {
        private const string PowerName = "Call to Death";

        public CallToDeath()
        {
            Name = PowerName;
            Text =
                "Attack two blights in your location at once. Make a single fight roll with +1 die, then divide the dice between blights and resolve as two separate attacks (losing Secrecy for each).";
        }

        public override void Learn(Hero hero)
        {
            base.Learn(hero);
            hero.AddAction(new CallToDeathAction());
        }

        private class CallToDeathAction : PowerAction, IRollHandler
        {
            public CallToDeathAction() : base(PowerName)
            {
            }

            public override void Act(Hero hero)
            {
                hero.ValidateState(HeroState.ChoosingAction);
                hero.SetRollHandler(this);
                hero.AddRollModifier(StaticRollBonus.Create(Name, RollType.Fight, 1));
                hero.ConflictState = new ConflictState
                {
                    ConflictType = ConflictType.Attack,
                    AvailableTactics = hero.GetAvailableFightTactics().GetInfo(hero),
                    AvailableTargets = hero.GetBlights().GetTargetInfo(),
                    MinTarget = 2,
                    MaxTarget = 2
                };
                hero.State = HeroState.SelectingTarget;
                hero.IsActionAvailable = false;
            }

            public RollState HandleRoll(Hero hero, RollState rollState)
            {
                return rollState;
            }

            public void AcceptRoll(Hero hero, RollState rollState)
            {
                hero.RemoveRollModifiers(Name);
                hero.State = HeroState.AssigningDice;
                hero.RemoveRollHandler(this);
            }
        }


        public override bool IsUsable(Hero hero)
        {
            return base.IsUsable(hero) && hero.GetBlights().Count() > 1;
        }
    }
}