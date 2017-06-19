using System.Linq;
using Slugburn.DarkestNight.Rules.Actions;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Powers;
using Slugburn.DarkestNight.Rules.Tactics;
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

        public class BlindingBlack : Bonus
        {
            public BlindingBlack()
            {
                Name = "Blinding Black";
                StartingPower = true;
                Text = "Exhaust after a Necromancer movement roll to prevent him from detecting any heroes, regardless of Secrecy.";
            }

            public override void Learn(Hero hero)
            {
                base.Learn(hero);
                var handler = new BlindingBlackTriggerHandler {HeroName = hero.Name};
                hero.Game.Triggers.Register(GameTrigger.NecromancerDetectsHeroes, handler);
            }

            public class BlindingBlackTriggerHandler : ITriggerHandler<Game>
            {
                public string Name => "Blinding Black";
                public string HeroName { get; set; }

                public void HandleTrigger(Game registrar, TriggerContext context)
                {
                    var hero = registrar.GetHero(HeroName);
                    var power = hero.GetPower(Name);
                    if (!power.IsUsable(hero)) return;
                    if (!hero.Player.AskUsePower(Name, power.Text)) return;
                    power.Exhaust(hero);
                    context.Cancel = true;
                }
            }
        }

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
                        AvailableTargets = hero.GetBlights(),
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


            public override bool IsUsable(Hero hero)
            {
                return base.IsUsable(hero) && hero.GetBlights().Count() > 1;
            }
        }

        class DarkVeil : Bonus
        {
            private const string PowerName = "Dark Veil";

            public DarkVeil()
            {
                Name = PowerName;
                StartingPower = true;
                Text =
                    "Exhaust at any time to ignore blights' effects until your next turn. *OR* Exhaust after you fail an attack on a blight to ignore its Defense.";
            }

            public override void Learn(Hero hero)
            {
                base.Learn(hero);
                hero.AddAction(new DarkVeilAction());
                hero.Triggers.Register(HeroTrigger.FailedAttack, new FailedAttackHandler());
            }

            private class FailedAttackHandler : ITriggerHandler<Hero>
            {
                public string Name => PowerName;
                public void HandleTrigger(Hero registrar, TriggerContext context)
                {
                    var hero = registrar;
                    var power = hero.GetPower(Name);
                    if (!power.IsUsable(hero)) return;
                    if (!hero.Player.AskUsePower(Name, power.Text)) return;
                    power.Exhaust(hero);
                    context.Cancel = true;
                }
            }

            private class DarkVeilAction : PowerAction
            {
                public DarkVeilAction() : base(PowerName)
                {
                }

                public override void Act(Hero hero)
                {
                    hero.Game.AddIgnoreBlight(IgnoreBlight.Create(Name, hero));
                    hero.Triggers.Register(HeroTrigger.StartTurn, new DarkVeilEnds());
                    hero.GetPower(Name).Exhaust(hero);
                }

                private class DarkVeilEnds : ITriggerHandler<Hero>
                {
                    public string Name => PowerName;
                    public void HandleTrigger(Hero registrar, TriggerContext context)
                    {
                        var hero = registrar;
                        hero.Game.RemoveIgnoreBlight(Name);
                        hero.Triggers.Unregister(HeroTrigger.StartTurn, Name);
                    }
                }
            }
        }

        class DeathMask : Bonus
        {
            private const string PowerName = "Death Mask";

            public DeathMask()
            {
                Name = PowerName;
                Text =
                    "You may choose not to lose Secrecy for attacking a blight (including use of the Call to Death power) or for starting your turn at the Necromancer's location.";
            }

            public override void Learn(Hero hero)
            {
                base.Learn(hero);
                hero.Triggers.Register(HeroTrigger.LoseSecrecy, new DeathMaskTriggerHandler());
            }

            private class DeathMaskTriggerHandler : ITriggerHandler<Hero>
            {
                public string Name => PowerName;
                public void HandleTrigger(Hero registrar, TriggerContext context)
                {
                    var hero = registrar;
                    var power = hero.GetPower(PowerName);
                    if (!power.IsUsable(hero)) return;
                    var sourceName = context.GetState<string>();
                    if (sourceName == "Attack" || sourceName == "Necromancer")
                        context.Cancel = true;
                }

            }
        }

        class FadeToBlack : Bonus
        {
            public FadeToBlack()
            {
                Name = "Fade to Black";
                Text = "+1 die in fights when Darkness is 10 or more. Another +1 die in fights when Darkness is 20 or more.";
            }

            public override void Learn(Hero hero)
            {
                base.Learn(hero);
                hero.AddRollModifier(new FadeToBlackRollModifer(Name));
            }

            private class FadeToBlackRollModifer : IRollModifier
            {
                private readonly string _powerName;

                public FadeToBlackRollModifer(string powerName)
                {
                    _powerName = powerName;
                }

                public int GetModifier(Hero hero, RollType rollType)
                {
                    var power = hero.GetPower(_powerName);
                    if (!power.IsUsable(hero)) return 0;
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

            public override void Learn(Hero hero)
            {
                base.Learn(hero);
                hero.Add(new PreventMovementEffect(location=>Exhausted && location==Location.Monastery));
            }

            public void Use(Hero hero)
            {
                if (!IsUsable(hero))
                    throw new PowerNotUsableException(this);
                hero.GainGrace(1, hero.DefaultGrace);
                Exhaust(hero);
            }

            public override bool IsUsable(Hero hero)
            {
                return base.IsUsable(hero) && hero.Grace < hero.DefaultGrace && hero.Location != Location.Monastery;
            }
        }

        class FalseOrders : ActionPower
        {
            private const string PowerName = "False Orders";

            public FalseOrders()
            {
                Name = PowerName;
                Text = "Move any number of blights from your location to one adjacent location, if this does not result in over 4 blights at one location.";
            }

            public override void Learn(Hero hero)
            {
                base.Learn(hero);
                hero.AddAction(new FalseOrdersAction());
            }

            private class FalseOrdersAction : PowerAction
            {
                public FalseOrdersAction() : base(PowerName)
                {
                }

                public override void Act(Hero hero)
                {
                    var space = hero.GetSpace();
                    var potentialDestinations = space.AdjacentLocations;
                    var destination = hero.Player.ChooseLocation(potentialDestinations);
                    if (destination == Location.None)
                        return;
                    var destinationSpace = hero.Game.Board[destination];
                    var maxMoveCount = 4 - hero.Game.Board[destination].Blights.Count();
                    var blights = hero.Player.ChooseBlights(space.Blights, 1, maxMoveCount);
                    if (!blights.Any())
                        return;
                    foreach (var blight in blights)
                    {
                        space.RemoveBlight(blight);
                        destinationSpace.AddBlight(blight);
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

            public override void Learn(Hero hero)
            {
                base.Learn(hero);
                hero.AddTactic(new FinalRestTactic(Name, 2));
                hero.AddTactic(new FinalRestTactic(Name, 3));
            }

            private class FinalRestTactic : PowerTactic, IRollHandler
            {
                public FinalRestTactic(string powerName, int diceCount)
                {
                    PowerName = powerName;
                    DiceCount = diceCount;
                    Type = TacticType.Fight;
                }

                public override string Name => $"{PowerName} ({DiceCount} dice)";

                public override void Use(Hero hero)
                {
                    base.Use(hero);
                    hero.AddRollHandler(this);
                }

                public void HandleRoll(Hero hero)
                {
                    if (hero.Roll.Any(x => x == 1))
                        hero.LoseGrace();
                    hero.RemoveRollHandler(this);
                }
            }
        }

        class ForbiddenArts : Bonus
        {
            private const string PowerName = "Forbidden Arts";

            public ForbiddenArts()
            {
                Name = PowerName;
                Text = "After a fight roll, add any number of dice, one at a time. For each added die that comes up a 1, +1 Darkness.";
            }

            public override void Learn(Hero hero)
            {
                base.Learn(hero);
                hero.AddAction(new ForbiddenArtsAction());
            }

            private class ForbiddenArtsAction : PowerAction
            {
                public ForbiddenArtsAction() : base(PowerName)
                {
                }

                public override void Act(Hero hero)
                {
                    var roll = hero.Player.RollOne();
                    if (roll == 1)
                        hero.Game.IncreaseDarkness();
                    hero.Roll.Add(roll);
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

                public void HandleRoll(Hero hero)
                {
                    var targetMight = hero.ConflictState.Targets.Select(x => new BlightFactory().Create(x)).Min(x => x.Might);
                    if (hero.Roll.Count(roll => roll >= targetMight) > 1)
                        hero.GainGrace(1, hero.DefaultGrace);
                    hero.RemoveRollHandler(this);
                }
            }
        }

        #endregion
    }

    internal class IgnoreBlight : IIgnoreBlight
    {
        public static IIgnoreBlight Create(string name, Hero hero)
        {
            return new IgnoreBlight {Name = name, HeroName = hero.Name};
        }

        public string HeroName { get; set; }
        public string Name { get; set; }
        public bool IsIgnoring(Hero hero, Blight blight)
        {
            return hero.Name == HeroName;
        }
    }
}
