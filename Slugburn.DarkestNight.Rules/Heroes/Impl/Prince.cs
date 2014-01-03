using Slugburn.DarkestNight.Rules.Powers;

namespace Slugburn.DarkestNight.Rules.Heroes.Impl
{
    public class Prince : Hero
    {
        public Prince()
        {
            Name = "Prince";
            DefaultGrace = 4;
            DefaultSecrecy = 3;
            Powers = new IPower[]
                     {
                         new Chapel(), 
                         new DivineRight(), 
                         new Inspire(), 
                         new Loyalty(), 
                         new Rebellion(),
                         new Resistance(), 
                         new SafeHouse(), 
                         new Scouts(), 
                         new SecretPassage(), 
                         new Strategy(), 
                     };
        }

        #region Powers

        class Chapel : Action
        {
            public Chapel()
            {
                Name = "Chapel";
                Text = "Spend 1 Secrecy to activate in your location.";
                ActiveText = "Heroes may pray there.";
            }

            public override bool IsUsable()
            {
                return base.IsUsable() && Hero.Secrecy > 0;
            }

            public override void Activate()
            {
                var space = Hero.GetSpace();
                var locationAction = new LocationAction(ActionType.Pray, this);
                Stash.Set(space, locationAction);

                space.Add(locationAction);
                Hero.LoseSecrecy();
            }

            public override void Deactivate()
            {
                base.Deactivate();
                var space = Stash.Get<ISpace>();
                var locationAction = Stash.Get<LocationAction>();
                space.Remove(locationAction);
            }
        }
        class DivineRight : Bonus
        {
            public DivineRight()
            {
                Name = "Divine Right";
                Text = "+1 to default Grace. Add 1 to each die when praying.";
            }

            public override void Activate()
            {
                Hero.DefaultGrace++;
                var bonus = new RollBonus(RollType.Pray, BonusType.Results, 1, this);
                Hero.Add(bonus);
                Stash.Set(bonus);
            }

            public override void Deactivate()
            {
                base.Deactivate();
                Hero.DefaultGrace--;
                var bonus = Stash.Get<RollBonus>();
                Hero.Remove(bonus);
            }
        }
        class Inspire : Action
        {
            public Inspire()
            {
                Name = "Inspire";
                StartingPower = true;
                Text = "Activate on a hero in your location.";
                ActiveText = "Deactivate before any die roll for +3d.";
            }

            public override void Activate()
            {
                Hero.SelectTarget<IHero>(TargetType.Hero)
                    .OnTarget(target =>
                    {
                        var bonus = new RollBonus(RollType.Any, BonusType.Dice, 3, this)
                        {
                            Auto = false,
                            OnUse = () => Deactivate()
                        };
                        target.Add(bonus);
                        Stash.Set(target, bonus);
                    });
            }

            public override void Deactivate()
            {
                base.Deactivate();
                var target = Stash.Get<IHero>();
                var bonus = Stash.Get<RollBonus>();
                target.Remove(bonus);
            }
        }
        class Loyalty : Bonus
        {
            public Loyalty()
            {
                Name = "Loyalty";
                StartingPower = true;
                Text = "+1d when eluding.";
            }

            public override void Activate()
            {
                var bonus = new RollBonus(RollType.Elude, BonusType.Dice, 1, this);
                Hero.Add(bonus);
                Stash.Set(bonus);
            }

            public override void Deactivate()
            {
                var bonus = Stash.Get<RollBonus>();
                Hero.Remove(bonus);
            }
        }
        class Rebellion : Tactic
        {
            public Rebellion()
                : base(TacticType.Fight)
            {
                Name = "Rebellion";
                Text = "Fight with 3d when attacking a blight or the Necromancer";
            }

            public override bool IsUsable()
            {
                return base.IsUsable() && (Hero.State == HeroState.AttackingBlight || Hero.State == HeroState.AttackingNecromancer);
            }

            public override void Activate()
            {
                Hero.SetDice(RollType.Fight, 3);
            }
        }
        class Resistance : Action
        {
            public Resistance()
            {
                Name = "Resistance";
                StartingPower = true;
                Text = "Spend 1 Secrecy to activate in your location.";
                ActiveText = "Heroes gain +1d in fights when attacking blights there.";
            }

            public override bool IsUsable()
            {
                return base.IsUsable() && Hero.Secrecy > 0;
            }

            public override void Activate()
            {
                Hero.LoseSecrecy();
                var space = Hero.GetSpace();
                var bonus = new RollBonus(RollType.Fight, BonusType.Dice, 1, this) { Restriction = hero => hero.State == HeroState.AttackingBlight };
                space.Add(bonus);
                Stash.Set(space, bonus);
            }

            public override void Deactivate()
            {
                base.Deactivate();
                var space = Stash.Get<ISpace>();
                var bonus = Stash.Get<RollBonus>();
                space.Remove(bonus);
            }
        }
        class SafeHouse : Action
        {
            public SafeHouse()
            {
                Name = "Safe House";
                Text = "Spend 2 Secrecy to activate in your location.";
                ActiveText = "Heroes gain 1 Secrecy (up to 5) when ending a turn there, and +1d when eluding there.";
            }

            public override void Activate()
            {
                var space = Hero.GetSpace();
                var effect = new TriggeredEffect(Trigger.EndOfTurn, "Gain 1 Secrecy (up to 5)", hero => hero.GainSecrecy(1, 5), this);
                var bonus = new RollBonus(RollType.Elude, BonusType.Dice, 1, this);
                space.Add(effect);
                space.Add(bonus);
                Stash.Set(space, effect, bonus);
            }

            public override void Deactivate()
            {
                var space = Stash.Get<ISpace>();
                var effect = Stash.Get<TriggeredEffect>();
                var bonus = Stash.Get<ISpace>();
                space.Remove(effect);
                space.Remove(bonus);
            }
        }
        class Scouts : Action
        {
            public Scouts()
            {
                Name = "Scouts";
                Text = "Spend 1 Secrecy to activate in your location.";
                ActiveText = "Heroes gain +1d in searches there.";
            }

            public override bool IsUsable()
            {
                return base.IsUsable() && Hero.Secrecy > 0;
            }

            public override void Activate()
            {
                var space = Hero.GetSpace();
                var bonus = new RollBonus(RollType.Search, BonusType.Dice, 1, this);
                Stash.Set(space, bonus);
                space.Add(bonus);
                Hero.LoseSecrecy();
            }

            public override void Deactivate()
            {
                var space = Stash.Get<ISpace>();
                var bonus = Stash.Get<RollBonus>();
                space.Remove(bonus);
            }
        }
        class SecretPassage : Action
        {
            public SecretPassage()
            {
                Name = "Secret Passage";
                Text = "Move to an adjacent location and gain 2 Secrecy (up to 5).";
            }

            public override void Activate()
            {
                Hero.SelectTarget<Location>(TargetType.AdjacentLocation)
                    .OnTarget(location =>
                    {
                        Hero.MoveTo(location);
                        Hero.GainSecrecy(2, 5);
                    });
            }
        }
        class Strategy : Tactic
        {
            public Strategy()
                : base(TacticType.Fight)
            {
                Name = "Strategy";
                StartingPower = true;
                Text = "Fight with 2d.";
            }

            public override bool IsUsable()
            {
                return base.IsUsable() && (Hero.State == HeroState.Fighting || Hero.State == HeroState.AttackingBlight || Hero.State == HeroState.AttackingNecromancer);
            }

            public override void Activate()
            {
                Hero.SetDice(RollType.Fight, 2);
            }
        }

        #endregion

    }
}
