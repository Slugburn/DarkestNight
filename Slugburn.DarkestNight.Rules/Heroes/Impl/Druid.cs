using System.Linq;
using Slugburn.DarkestNight.Rules.Actions;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Powers;
using Slugburn.DarkestNight.Rules.Rolls;
using Slugburn.DarkestNight.Rules.Tactics;
using Slugburn.DarkestNight.Rules.Triggers;

namespace Slugburn.DarkestNight.Rules.Heroes.Impl
{
    public class Druid : Hero
    {
        public Druid()
            : base(
                "Druid", 5, 6, new AnimalCompanion(), new Camouflage(), new Celerity(), new RavenForm(), new SpriteForm(), new TreeForm(), new Tranquility(),
                new Vines(), new Visions(), new WolfForm())
        {
        }

        private interface IDruidForm : IPower, IActivateable
        {
            
        }

        class AnimalCompanion : TacticPower
        {
            public AnimalCompanion() : base(TacticType.Fight)
            {
                Name = "Animal Companion";
                StartingPower = true;
                Text = "Fight with 2 dice. Exhaust if you fail.";
            }

            public override void Learn(Hero hero)
            {
                base.Learn(hero);
                hero.AddTactic(new AnimalCompanionTactic());
            }

            private class AnimalCompanionTactic : PowerTactic
            {
                public AnimalCompanionTactic()
                {
                    PowerName = "Animal Companion";
                    Type = TacticType.Fight;
                    DiceCount = 2;
                }
            }
        }

        class Camouflage : TacticPower
        {
            public Camouflage() : base(TacticType.Elude)
            {
                Name = "Camouflage";
                StartingPower = true;
                Text = "Elude with 2 dice.";
            }

            public override void Learn(Hero hero)
            {
                base.Learn(hero);
                hero.AddTactic(new CamouflageTactic());
            }

            private class CamouflageTactic : PowerTactic
            {
                public CamouflageTactic()
                {
                    PowerName = "Camouflage";
                    Type = TacticType.Elude;
                    DiceCount = 2;
                }
            }
        }

        class Celerity : ActionPower
        {
            private const string PowerName = "Celerity";

            public Celerity()
            {
                Name = PowerName;
                Text = "Deactivate all Forms. Travel. Optionally activate one of your Forms.";
            }

            public override void Learn(Hero hero)
            {
                base.Learn(hero);
                hero.AddAction(new CelerityAction());
            }

            private class CelerityAction : PowerAction
            {
                public CelerityAction() : base(PowerName)
                {
                }

                public override void Act(Hero hero)
                {
                    hero.ValidateState(HeroState.ChoosingAction);
                    DeactivateAllForms(hero);
                    hero.State = HeroState.SelectingLocation;
                    hero.SetLocationSelectedHandler(new CelerityLocationSelectedHandler());
                }

                private class CelerityLocationSelectedHandler : ILocationSelectedHandler
                {
                    public void Handle(Hero hero, Location location)
                    {
                        // Move to selected location
                        hero.MoveTo(location);

                        // Allow player to pick a new form
                        hero.State = HeroState.ChoosingAction;
                        var formActions = hero.GetPowers<IDruidForm>().Where(x => x.IsUsable(hero)).Select(x => x.Name);
                        var continueAction = new CelerityContinueAction();
                        var availableActions = formActions.Concat(new[] {continueAction.Name}).ToList();
                        hero.AddAction(continueAction);
                        hero.AvailableActions = availableActions;
                    }

                }
                private class CelerityContinueAction : IAction
                {
                    public string Name => "Continue";

                    public void Act(Hero hero)
                    {
                        hero.RemoveAction(Name);
                        hero.IsActionAvailable = false;
                    }

                    public bool IsAvailable(Hero hero)
                    {
                        return true;
                    }
                }
            }
        }

        class RavenForm : DruidFormPower
        {
            public RavenForm()
            {
                Name = "Raven Form";
                Text = "Deactivate all Forms. Optionally activate.";
                ActiveText = "+1 die in searches. When you travel, you may move two spaces. You cannot gain Grace.";
            }

            public override void Activate(Hero hero)
            {
                base.Activate(hero);
                hero.TravelSpeed = 2;
                hero.AddRollModifier(StaticRollBonus.Create(Name, RollType.Search, 1));
            }

            public override bool Deactivate(Hero hero)
            {
                if (!base.Deactivate(hero)) return false;
                hero.TravelSpeed = 1;
                hero.RemoveRollModifier(Name);
                return true;
            }
        }

        private class DruidFormPower : ActivateablePower, IDruidForm
        {
            public override void Learn(Hero hero)
            {
                base.Learn(hero);
                AddFormActions(hero, Name);
            }

            public override void Activate(Hero hero)
            {
                base.Activate(hero);
                hero.IsActionAvailable = false;
                hero.CanGainGrace = false;
            }

            public override bool Deactivate(Hero hero)
            {
                if (!base.Deactivate(hero)) return false;
                hero.CanGainGrace = true;
                return true;
            }
        }

        class SpriteForm : DruidFormPower
        {
            private const string PowerName = "Sprite Form";

            public SpriteForm()
            {
                Name = PowerName;
                StartingPower = true;
                Text = "Deactivate all Forms. Optionally activate.";
                ActiveText = "Ignore blights' effects unless the Necromancer is present. You cannot gain Grace.";
            }

            public override void Activate(Hero hero)
            {
                base.Activate(hero);
                hero.Game.AddIgnoreBlight(new SpriteFormIgnoreBlight {HeroName = hero.Name});
            }

            private class SpriteFormIgnoreBlight : IIgnoreBlight
            {
                public string Name => PowerName;
                public string HeroName { get; set; }

                public bool IsIgnoring(Hero hero, Blight blight)
                {
                    return hero.Name == HeroName && hero.Location != hero.Game.Necromancer.Location;
                }
            }

            public override bool Deactivate(Hero hero)
            {
                if (!base.Deactivate(hero)) return false;
                hero.Game.RemoveIgnoreBlight(Name);
                return true;
            }
        }

        private class ActivateForm : PowerAction
        {
            public override void Act(Hero hero)
            {
                hero.ValidateState(HeroState.ChoosingAction);
                DeactivateAllForms(hero);
                var power = (IDruidForm) hero.GetPower(Name);
                power.Activate(hero);
            }
        }

        private class DeactivateForm : IAction
        {
            public const string ActionName = "Deactivate Form";
            public string Name => ActionName;
            public void Act(Hero hero)
            {
                DeactivateAllForms(hero);
                hero.IsActionAvailable = false;
            }

            public bool IsAvailable(Hero hero)
            {
                return hero.IsActionAvailable && hero.GetPowers<IDruidForm>().Any(x => x.IsActive);
            }
        }

        class Tranquility : Bonus
        {
            public Tranquility()
            {
                Name = "Tranquility";
                Text = "+3 to default Grace.";
            }

            public override void Learn(Hero hero)
            {
                base.Learn(hero);
                hero.DefaultGrace += 3;
            }
        }

        class TreeForm : DruidFormPower
        {
            private const string PowerName = "Tree Form";

            public TreeForm()
            {
                Name = PowerName;
                Text = "Deactivate all Forms. Optionally activate.";
                ActiveText = "Gain 2 Grace (up to default) at the start of your turn. Your actions can only be to hide or use a Druid power.";
            }

            public override void Activate(Hero hero)
            {
                base.Activate(hero);
                hero.CanGainGrace = true;
                hero.Triggers.Register(HeroTrigger.StartTurn, new TreeFormStartTurnHandler());
                hero.AddActionFilter(PowerName, HeroState.ChoosingAction, new [] { "Hide", "Tree Form", "Celerity", "Raven Form", "Sprite Form", "Wolf Form", "Deactivate Form" });
            }

            public override bool Deactivate(Hero hero)
            {
                if (!base.Deactivate(hero)) return false;
                hero.Triggers.Unregister(HeroTrigger.StartTurn, PowerName);
                hero.RemoveActionFilter(PowerName);
                return true;
            }

            private class TreeFormStartTurnHandler : ITriggerHandler<Hero>
            {
                public string Name => PowerName;
                public void HandleTrigger(Hero registrar, TriggerContext context)
                {
                    registrar.GainGrace(2, registrar.DefaultGrace);
                }
            }
        }

        class Vines : TacticPower
        {
            public Vines() : base(TacticType.Fight | TacticType.Elude)
            {
                Name = "Vines";
                Text = "Exhaust to fight or elude with 4 dice.";
            }

            public override void Learn(Hero hero)
            {
                base.Learn(hero);
                hero.AddTactic(new VinesTactic {PowerName = Name, Type = TacticType.Elude, DiceCount = 4});
                hero.AddTactic(new VinesTactic {PowerName = Name, Type = TacticType.Fight, DiceCount = 4});
            }

            private class VinesTactic : PowerTactic
            {
                public override string Name => $"{PowerName} [{Type}]";

                public override void Use(Hero hero)
                {
                    base.Use(hero);
                    hero.AddRollHandler(new ExhaustPowerRollHandler {PowerName = PowerName});
                }

                private class ExhaustPowerRollHandler : IRollHandler
                {
                    public void HandleRoll(Hero hero)
                    {
                        hero.GetPower(PowerName).Exhaust(hero);
                    }

                    public string PowerName { get; set; }
                }
            }
        }

        class Visions : Bonus
        {
            public Visions()
            {
                Name = "Visions";
                StartingPower = true;
                Text = "Exhaust after you draw an event card to discard it without effect.";
            }
        }

        class WolfForm : DruidFormPower
        {
            public WolfForm()
            {
                Name = "Wolf Form";
                Text = "Deactivate all Forms. Optionally activate.";
                ActiveText = "+1 die in fights. +1 die when eluding. You cannot gain Grace.";
            }
        }

        private static void AddFormActions(Hero hero, string powerName)
        {
            if (!hero.HasAction(DeactivateForm.ActionName))
                hero.AddAction(new DeactivateForm());
            hero.AddAction(new ActivateForm { Name = powerName });
        }

        private static void DeactivateAllForms(Hero hero)
        {
            hero.Powers.Where(x => x is IDruidForm).Cast<IDruidForm>().ToList().ForEach(x => x.Deactivate(hero));
        }
    }

}