using System;
using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Powers;
using Slugburn.DarkestNight.Rules.Triggers;

namespace Slugburn.DarkestNight.Rules.Heroes.Impl
{
    public class Acolyte : Hero
    {
        public Acolyte()
            : base(
                "Acolyte", 3, 7, new BlindingBlack(), new CallToDeath(), new DarkVeil(), new DeathMask(), new FadeToBlack(), new FalseLife(), new FalseOrders(),
                new FinalRest(), new ForbiddenArts(), new LeechLife())
        {
        }

        #region Powers

        public class BlindingBlack : Bonus, ITriggerHandler
        {
            public BlindingBlack()
            {
                Name = "Blinding Black";
                StartingPower = true;
                Text = "Exhaust after a Necromancer movement roll to prevent him from detecting any heroes, regardless of Secrecy.";
            }

            public override void Learn()
            {
                base.Learn();
                Hero.Game.Triggers.Register(this, GameTrigger.NecromancerDetectsHeroes);
            }

            public void HandleTrigger(TriggerContext context, string tag)
            {
                if (!IsUsable()) return;
                if (!Hero.Player.AskUsePower(Name, Text)) return;
                Exhaust();
                context.Cancel = true;
            }
        }

        class CallToDeath : ActionPower
        {
            public CallToDeath()
            {
                Name = "Call to Death";
                Text =
                    "Attack two blights in your location at once. Make a single fight roll with +1 die, then divide the dice between blights and resolve as two separate attacks (losing Secrecy for each).";
            }

            public override void Activate()
            {
                base.Activate();
                var hero = (Hero)Hero;
                var player = hero.Player;
                var blights = hero.GetBlights().Select(x=>x.Type).ToList();
                var selectedBlights = blights.Count == 2
                    ? blights
                    : player.ChooseBlights(blights, 2);
                var selectedTactic = hero.SelectTactic();
                const int bonusDice = 1;
                var dieCount = selectedTactic.GetDieCount() + bonusDice;
                var rolls = selectedTactic.RollDice(dieCount).ToList();
                foreach (var blightType in selectedBlights)
                {
                    var blight = hero.GetBlights().First(x => x.Type == blightType);
                    var assignedRoll =  player.AssignRollToBlight(blightType, rolls);
                    rolls.Remove(assignedRoll);
                    hero.ResolveAttack(blight, assignedRoll);
                }
            }

            public override bool IsUsable()
            {
                return base.IsUsable() && Hero.GetBlights().Count() > 1;
            }
        }

        class DarkVeil : Bonus, IBonusAction, ITriggerHandler
        {
            public DarkVeil()
            {
                Name = "Dark Veil";
                StartingPower = true;
                Text =
                    "Exhaust at any time to ignore blights' effects until your next turn. *OR* Exhaust after you fail an attack on a blight to ignore its Defense.";
            }

            public override void Learn()
            {
                base.Learn();
                Hero.Triggers.Register(this, HeroTrigger.FailedAttack);
            }

            public void Use()
            {
                if (!IsUsable())
                    throw new PowerNotUsableException(this);

                Hero.Add(new IgnoreBlightEffect(blight => true, Name));
                Hero.Triggers.Register(this, HeroTrigger.StartTurn);

                Exhaust();
            }

            public void HandleTrigger(TriggerContext context, string tag)
            {
                switch (tag)
                {
                    case "FailedAttack":
                        if (!IsUsable()) return;
                        if (!Hero.Player.AskUsePower(Name, Text)) return;
                        Exhaust();
                        context.Cancel = true;
                        break;
                    case "StartTurn":
                        Hero.RemoveBySource<IgnoreBlightEffect>(Name);
                        break;
                }
            }
        }

        class DeathMask : Bonus, ITriggerHandler
        {
            public DeathMask()
            {
                Name = "Death Mask";
                Text =
                    "You may choose not to lose Secrecy for attacking a blight (including use of the Call to Death power) or for starting your turn at the Necromancer's location.";
            }

            public override void Learn()
            {
                base.Learn();
                Hero.Triggers.Register(this, HeroTrigger.LoseSecrecy);
            }

            public void HandleTrigger(TriggerContext context, string tag)
            {
                if (!IsUsable()) return;
                var sourceName = context.SourceName;
                if (sourceName == "Attack" || sourceName == "Necromancer")
                    context.Cancel = true;
            }
        }

        class FalseLife : Bonus, IBonusAction
        {
            public FalseLife()
            {
                Name = "False Life";
                StartingPower = true;
                Text = "Exhaust at any time while not at the Monastery to gain 1 Grace (up to default). You may not enter the Monastery while this power is exhausted.";
            }

            public override void Learn()
            {
                base.Learn();
                Hero.Add(new PreventMovementEffect(location=>Exhausted && location==Location.Monastery));
            }

            public void Use()
            {
                if (!IsUsable())
                    throw new PowerNotUsableException(this);
                Hero.Grace = Math.Min(Hero.Grace+1, Hero.DefaultGrace);
                Exhaust();
            }

            public override bool IsUsable()
            {
                return base.IsUsable() && Hero.Grace < Hero.DefaultGrace && Hero.Location != Location.Monastery;
            }
        }

        class FadeToBlack : Bonus, IDieBonus
        {
            public FadeToBlack()
            {
                Name = "Fade to Black";
                Text = "+1 die in fights when Darkness is 10 or more. Another +1 die in fights when Darkness is 20 or more.";
            }

            public void ModifyDice(RollContext context)
            {
                if (!IsUsable()) return;
                if (Hero.Game.Darkness >= 10)
                    context.DieCount++;
                if (Hero.Game.Darkness >= 20)
                    context.DieCount++;
            }
        }

        class FalseOrders : ActionPower
        {
            public FalseOrders()
            {
                Name = "False Orders";
                Text = "Move any number of blights from your location to one adjacent location, if this does not result in over 4 blights at one location.";
            }

            public override void Activate()
            {
                throw new System.NotImplementedException();
            }
        }

        class FinalRest : Tactic
        {
            public FinalRest() : base(TacticType.Fight)
            {
                Name = "Final Rest";
                StartingPower = true;
                Text = "Fight with 2d or 3d. If any die comes up a 1, lose 1 Grace.";
                base.GetDieCount = ChooseDieCount;
            }

            private int ChooseDieCount()
            {
                return Hero.Player.ChooseDieCount(2, 3);
            }

            internal override IEnumerable<int> RollDice(int count)
            {
                var roll = base.RollDice(count).ToList();
                if (roll.Any(x => x == 1))
                    Hero.LoseGrace();
                return roll;
            }
        }

        class ForbiddenArts : Bonus
        {
            public ForbiddenArts()
            {
                Name = "Forbidden Arts";
                Text = "After a fight roll, add any number of dice, one at a time. For each added die that comes up a 1, +1 Darkness.";
            }
        }

        class LeechLife: Tactic
        {
            public LeechLife() : base(TacticType.Fight, 3)
            {
                Name = "Leech Life";
                Text = "Exhaust while not at the Monastery to fight with 3 dice. Gain 1 Grace (up to default) if you roll 2 successes. You may not enter the Monastery while this power is exhausted.";
            }
        }

        #endregion
    }
}
