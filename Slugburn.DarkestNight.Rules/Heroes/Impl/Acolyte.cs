using System;
using System.Linq;
using Slugburn.DarkestNight.Rules.Actions;
using Slugburn.DarkestNight.Rules.Extensions;
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
                Game.Triggers.Register(this, GameTrigger.NecromancerDetectsHeroes);
            }

            public void HandleTrigger(TriggerContext context, string tag)
            {
                if (!IsUsable()) return;
                if (!Player.AskUsePower(Name, Text)) return;
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

            public override void Learn()
            {
                base.Learn();
                Hero.AddAction(new CallToDeathAction());
            }

            private class CallToDeathAction : IAction, IRollHandler
            {
                public string Name  => "Call to Death";

                public void Act(Hero hero)
                {
                    hero.ValidateState(HeroState.ChoosingAction);
                    hero.SetRollClient(this);
                    hero.AddRollModifier(new StaticRollBonus(Name, TacticType.Fight, 1));
                    hero.ConflictState = new ConflictState
                    {
                        TacticType = TacticType.Fight,
                        AvailableFightTactics = hero.GetAvailableFightTactics().GetInfo(hero),
                        AvailableTargets = hero.GetSpace().Blights.Select(x => x.Type).ToList(),
                        MinTarget = 2,
                        MaxTarget = 2
                    };
                    hero.State = HeroState.SelectingTarget;
                    hero.IsActionAvailable = false;
                }

                public void HandleRoll(Hero hero)
                {
                    hero.RemoveRollModifier(Name);
                    hero.State = HeroState.AssigningDice;
                    hero.RemoveRollHandler(this);
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
                        if (!Player.AskUsePower(Name, Text)) return;
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
                var sourceName = context.GetState<string>();
                if (sourceName == "Attack" || sourceName == "Necromancer")
                    context.Cancel = true;
            }
        }

        class FadeToBlack : Bonus
        {
            public FadeToBlack()
            {
                Name = "Fade to Black";
                Text = "+1 die in fights when Darkness is 10 or more. Another +1 die in fights when Darkness is 20 or more.";
            }

            public override void Learn()
            {
                base.Learn();
                Hero.AddRollModifier(new FadeToBlackRollModifer(Name));
            }

            private class FadeToBlackRollModifer : IRollModifier
            {
                private readonly string _powerName;

                public FadeToBlackRollModifer(string powerName)
                {
                    _powerName = powerName;
                }

                public int GetModifier(Hero hero)
                {
                    var power = hero.GetPower(_powerName);
                    if (!power.IsUsable()) return 0;
                    var game = hero.Game;
                    if (game.Darkness < 10) return 0;
                    if (game.Darkness < 20) return 1;
                    return 2;
                }

                public string Name => _powerName;
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

        class FalseOrders : ActionPower
        {
            public FalseOrders()
            {
                Name = "False Orders";
                Text = "Move any number of blights from your location to one adjacent location, if this does not result in over 4 blights at one location.";
            }

            public override void Learn()
            {
                base.Learn();
                Hero.AddAction(new FalseOrdersAction());
            }

            private class FalseOrdersAction : IAction
            {
                public string Name => "False Orders";
                public void Act(Hero hero)
                {
                    var space = hero.GetSpace();
                    var potentialDestinations = space.AdjacentLocations;
                    var destination = hero.Player.ChooseLocation(potentialDestinations);
                    if (destination == Location.None)
                        return;
                    var destinationSpace = hero.Game.Board[destination];
                    var maxMoveCount = 4 - hero.Game.Board[destination].Blights.Count();
                    var blights = hero.Player.ChooseBlights(space.Blights.Select(x => x.Type).ToList(), 1, maxMoveCount);
                    if (!blights.Any())
                        return;
                    foreach (var blight in blights)
                    {
                        var blightObj = space.GetBlight(blight);
                        space.RemoveBlight(blightObj);
                        destinationSpace.AddBlight(blightObj);
                    }
                    hero.IsActionAvailable = false;
                }

            }
        }

        class FinalRest : TacticPower
        {
            public FinalRest() : base(TacticType.Fight)
            {
                Name = "Final Rest";
                StartingPower = true;
                Text = "Fight with 2d or 3d. If any die comes up a 1, lose 1 Grace.";
            }

            public override void Learn()
            {
                base.Learn();
                Hero.AddFightTactic(new FinalRestTactic(Name, 2));
                Hero.AddFightTactic(new FinalRestTactic(Name, 3));
            }

            private class FinalRestTactic : PowerTactic, IRollHandler
            {
                public FinalRestTactic(string powerName, int diceCount)
                {
                    PowerName = powerName;
                    DiceCount = diceCount;
                }

                public override string Name => $"{PowerName} ({DiceCount} dice)";

                public override void Use(Hero hero)
                {
                    base.Use(hero);
                    hero.AddRollHandler(this);
                }

                public void HandleRoll(Hero hero)
                {
                    if (hero.ConflictState.Roll.Any(x => x == 1))
                        hero.LoseGrace();
                    hero.RemoveRollHandler(this);
                }
            }
        }

        class ForbiddenArts : Bonus
        {
            public ForbiddenArts()
            {
                Name = "Forbidden Arts";
                Text = "After a fight roll, add any number of dice, one at a time. For each added die that comes up a 1, +1 Darkness.";
            }

            public override void Learn()
            {
                base.Learn();
                Hero.AddAction(new ForbiddenArtsAction());
            }

            private class ForbiddenArtsAction : IAction
            {
                public string Name => "Forbidden Arts";

                public void Act(Hero hero)
                {
                    var roll = hero.Player.RollOne();
                    if (roll == 1)
                        hero.Game.IncreaseDarkness();
                    hero.ConflictState.Roll.Add(roll);
                }
            }
        }

        class LeechLife: TacticPower
        {
            public LeechLife() : base(TacticType.Fight, 3)
            {
                Name = "Leech Life";
                Text = "Exhaust while not at the Monastery to fight with 3 dice. Gain 1 Grace (up to default) if you roll 2 successes. You may not enter the Monastery while this power is exhausted.";
            }

            public override void Learn()
            {
                base.Learn();
                Hero.Add(new PreventMovementEffect(location => location == Location.Monastery && Exhausted));
            }
        }

        #endregion
    }
}
