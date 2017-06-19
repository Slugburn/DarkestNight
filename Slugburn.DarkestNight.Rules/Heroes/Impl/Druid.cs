using System.Linq;
using Slugburn.DarkestNight.Rules.Actions;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Powers;

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

        private interface IForm : IPower, IActivateable
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

            private class CelerityAction : IAction
            {
                public string Name => PowerName;

                public void Act(Hero hero)
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
                        var formActions = hero.GetPowers<IForm>().Where(x => x.IsUsable(hero)).Select(x => x.Name);
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
                }
            }
        }

        class RavenForm : ActivateablePower , IForm
        {
            public RavenForm()
            {
                Name = "Raven Form";
                Text = "Deactivate all Forms. Optionally activate.";
                ActiveText = "+1 die in searches. When you travel, you may move two spaces. You cannot gain Grace.";
            }

            public override void Learn(Hero hero)
            {
                base.Learn(hero);
                AddFormActions(hero, Name);
            }

        }

        class SpriteForm : ActivateablePower, IForm
        {
            private const string PowerName = "Sprite Form";

            public SpriteForm()
            {
                Name = PowerName;
                StartingPower = true;
                Text = "Deactivate all Forms. Optionally activate.";
                ActiveText = "Ignore blights' effects unless the Necromancer is present. You cannot gain Grace.";
            }

            public override void Learn(Hero hero)
            {
                base.Learn(hero);
                AddFormActions(hero, Name);
            }

            public override void Activate(Hero hero)
            {
                base.Activate(hero);
                hero.Game.AddIgnoreBlight(new SpriteFormIgnoreBlight {HeroName = hero.Name});
                hero.IsActionAvailable = false;
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

            public override void Deactivate(Hero hero)
            {
                base.Deactivate(hero);
                hero.Game.RemoveIgnoreBlight(Name);
            }
        }

        private class ActivateForm : IAction
        {
            public string Name { get; set; }

            public void Act(Hero hero)
            {
                hero.ValidateState(HeroState.ChoosingAction);
                DeactivateAllForms(hero);
                var power = (IForm) hero.GetPower(Name);
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
        }


        class Tranquility : Bonus
        {
            public Tranquility()
            {
                Name = "Tranquility";
                Text = "+3 to default Grace.";
            }
        }

        class TreeForm : ActivateablePower, IForm
        {
            public TreeForm()
            {
                Name = "Tree Form";
                Text = "Deactivate all Forms. Optionally activate.";
                ActiveText = "Gain 2 Grace (up to default) at the start of your turn. Your actions can only be to hide or use a Druid power.";
            }
        }

        class Vines : TacticPower
        {
            public Vines() : base(TacticType.Fight | TacticType.Elude)
            {
                Name = "Vines";
                Text = "Exhaust to fight or elude with 4 dice.";
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

        class WolfForm : ActivateablePower, IForm
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
            hero.Powers.Where(x => x is IForm).Cast<IForm>().ToList().ForEach(x => x.Deactivate(hero));
        }
    }
}