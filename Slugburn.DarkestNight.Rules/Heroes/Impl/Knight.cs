using System.Linq;
using Slugburn.DarkestNight.Rules.Actions;
using Slugburn.DarkestNight.Rules.Powers;
using Slugburn.DarkestNight.Rules.Rolls;
using Slugburn.DarkestNight.Rules.Tactics;
using Slugburn.DarkestNight.Rules.Triggers;

namespace Slugburn.DarkestNight.Rules.Heroes.Impl
{
    public class Knight : Hero
    {
        public Knight()
            : base(
                "Knight", 5, 6, new Charge(), new ConsecratedBlade(), new HardRide(), new HolyMantle(), new OathOfDefense(), new OathOfPurging(),
                new OathOfValor(), new OathOfVengeance(), new RecklessAbandon(), new Sprint())
        {
        }

        class Charge : TacticPower
        {
            private const string PowerName = "Charge";

            public Charge() : base(TacticType.Fight)
            {
                Name = PowerName;
                StartingPower = true;
                Text = "Fight with 2 dice.";
            }

            public override void Learn(Hero hero)
            {
                base.Learn(hero);
                hero.AddTactic(new PowerTactic {PowerName = PowerName, Type = TacticType.Fight, DiceCount = 2});
            }
        }

        class ConsecratedBlade : Bonus
        {
            private const string PowerName = "Consecrated Blade";

            public ConsecratedBlade()
            {
                Name = PowerName;
                Text = "+1 dice in fights.";
            }

            public override void Learn(Hero hero)
            {
                base.Learn(hero);
                hero.AddRollModifier(new ConsecratedBladeRollModifer());
            }

            private class ConsecratedBladeRollModifer : IRollModifier
            {
                public string Name => PowerName;

                public int GetModifier(Hero hero, RollType rollType)
                {
                    if (rollType != RollType.Fight) return 0;
                    var power = hero.GetPower(PowerName);
                    if (!power.IsUsable(hero)) return 0;
                    return 1;
                }

            }
        }

        class HardRide : ActionPower
        {
            public HardRide()
            {
                Name = "Hard Ride";
                StartingPower = true;
                Text = "Move twice, but gain no Secrecy.";
            }

            public override void Learn(Hero hero)
            {
                base.Learn(hero);
                hero.AddAction(new HardRideAction {Name= Name});
            }

            private class HardRideAction : PowerAction
            {
                public override void Act(Hero hero)
                {
                    hero.State = HeroState.Moving;
                    hero.AvailableMovement = 2;
                    hero.IsActionAvailable = false;
                }
            }
        }

        class HolyMantle : Bonus
        {
            private const string PowerName = "Holy Mantle";

            public HolyMantle()
            {
                Name = PowerName;
                Text = "+1 to default Grace. Add 1 to each die when praying.";
            }

            public override void Learn(Hero hero)
            {
                base.Learn(hero);
                hero.DefaultGrace += 1;
                hero.Triggers.Register(HeroTrigger.AfterRoll, new HolyMantleAfterRoll());
            }

            private class HolyMantleAfterRoll : ITriggerHandler<Hero>
            {
                public string Name => PowerName;
                public void HandleTrigger(Hero registrar, TriggerContext context)
                {
                    var hero = registrar;
                    if (hero.State != HeroState.Praying) return;
                    var power = hero.GetPower(PowerName);
                    if (!power.IsUsable(hero)) return;

                    // Increase each die by 1
                    hero.Roll = hero.Roll.Select(x => x + 1).ToList();
                }
            }
        }

        interface IOath: IPower, IActivateable
        {
            string FulfillText { get; }
            string BreakText { get; }
        }

        abstract class Oath : ActivateablePower, IOath
        {
            protected Oath()
            {
                Text = "If no Oaths are active, activate until you fulfill or break.";
            }

            public override void Learn(Hero hero)
            {
                base.Learn(hero);
                hero.AddAction(new ActivateOathAction {Name = Name});
            }

            private class ActivateOathAction : PowerAction
            {
                public override void Act(Hero hero)
                {
                    var power = (Oath) hero.GetPower(Name);
                    power.Activate(hero);
                }
            }

            public override bool Deactivate(Hero hero)
            {
                if (!base.Deactivate(hero)) return false;
                hero.Triggers.UnregisterAll(Name);
                hero.Game.Triggers.UnregisterAll(Name);
                return true;
            }

            public string FulfillText { get; set; }
            public string BreakText { get; set; }

            public override bool IsUsable(Hero hero)
            {
                return base.IsUsable(hero) 
                    // Can not activate if any other oath powers are active
                    && !hero.Powers.Where(x=>x is IOath).Cast<IOath>().Any(x=>x.IsActive);
            }
        }

        class OathOfDefense : Oath
        {
            private const string PowerName = "Oath of Defense";

            public OathOfDefense()
            {
                Name = PowerName;
                ActiveText = "Gain 1 Grace (up to default) at start of turn.";
                FulfillText = "No blights at location; You gain 1 Grace.";
                BreakText = "Leave location; you lose all Grace.";
            }

            public override void Activate(Hero hero)
            {
                base.Activate(hero);
                // fullfill immediately if no blights are at the current location
                if (!hero.GetBlights().Any())
                {
                    FulfillOath(hero);
                }
                else
                {
                    hero.Triggers.Register(HeroTrigger.StartTurn, new OathOfDefenseActive() {Name=Name});
                    hero.Game.Triggers.Register(GameTrigger.BlightDestroyed, new OathOfDefenseFulfilled {Name = Name, HeroName = hero.Name});
                    hero.Triggers.Register(HeroTrigger.ChangeLocation, new OathOfDefenseBroken() {Name=Name});
                }
                hero.IsActionAvailable = false;
            }

            private class OathOfDefenseFulfilled : GameTriggerHandler
            {
                public string HeroName { get; set; }

                public override void HandleTrigger(Game game, TriggerContext context)
                {
                    var location = context.GetState<Location>();
                    var hero = game.GetHero(HeroName);
                    if (location != hero.Location) return;
                    var space = game.Board[location];
                    if (space.Blights.Any()) return;
                    var power = (OathOfDefense) hero.GetPower(PowerName);
                    power.FulfillOath(hero);
                }
            }

            private void FulfillOath(Hero hero)
            {
                hero.GainGrace(1, int.MaxValue);
                Deactivate(hero);
            }

            private class OathOfDefenseActive : HeroTriggerHandler
            {
                public override void HandleTrigger(Hero registrar, TriggerContext context)
                {
                    var hero = registrar;
                    hero.GainGrace(1, hero.DefaultGrace);
                }
            }
            private class OathOfDefenseBroken : HeroTriggerHandler
            {
                public override void HandleTrigger(Hero hero, TriggerContext context)
                {
                    hero.LoseGrace(hero.Grace);
                    var power = (OathOfDefense) hero.GetPower(Name);
                    power.Deactivate(hero);
                }
            }
        }

        class OathOfPurging: Oath
        {
            public OathOfPurging()
            {
                Name = "Oath of Purging";
                StartingPower = true;
                ActiveText = "+2 dice in fights when attacking blights.";
                FulfillText = "Destroy a blight; you gain 1 Grace.";
                BreakText = "Enter the Monastery; you lose 1 Grace.";
            }
        }

        class OathOfValor: Oath
        {
            public OathOfValor()
            {
                Name = "Oath of Valor";
                ActiveText = "+1 die in fights.";
                FulfillText = "Win a fight; You may activate any Oath immediately.";
                BreakText = "Attempt to elude; you lose 1 Grace.";
            }

        }

        class OathOfVengeance: Oath
        {
            public OathOfVengeance()
            {
                Name = "Oath of Vengeance";
                Text = "If no Oaths are active, activate until you fulfill or break.";
                ActiveText = "Add 1 to highest die when fighting the Necormancer.";
                FulfillText = "Win fight versus the Necromancer; you get a free action.";
                BreakText = "Hide or search; you lose 1 Grace.";
            }

        }

        class RecklessAbandon: TacticPower
        {
            public RecklessAbandon() : base(TacticType.Fight)
            {
                Name = "Reckless Abandon";
                Text = "Fight with 4 dice. Lose 1 Grace if you roll fewer than 2 successes.";
            }
        }

        class Sprint : TacticPower
        {
            public Sprint() : base(TacticType.Elude)
            {
                Name = "Sprint";
                StartingPower = true;
                Text = "Elude with 2 dice.";
            }
        }
    }
}
