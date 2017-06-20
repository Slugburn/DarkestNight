using Slugburn.DarkestNight.Rules.Powers;

namespace Slugburn.DarkestNight.Rules.Heroes.Impl
{
    public class Prince : Hero
    {
    
        #region Powers

        class Chapel : ActionPower
        {
            public Chapel()
            {
                Name = "Chapel";
                Text = "Spend 1 Secrecy to activate in your location.";
                ActiveText = "Heroes may pray there.";
            }

            public override bool IsUsable(Hero hero)
            {
                return base.IsUsable(hero) && hero.Secrecy > 0;
            }

//            public override void Activate()
//            {
//                var space = Hero.GetSpace();
//                var locationAction = new LocationAction(ActionType.Pray, this);
//                Stash.Add(space, locationAction);
//
//                space.Add(locationAction);
//                Hero.LoseSecrecy();
//            }
//
//            public override void Deactivate()
//            {
//                base.Deactivate();
//                var space = Stash.Get<ISpace>();
//                var locationAction = Stash.Get<LocationAction>();
//                space.Remove(locationAction);
//            }
        }
        class DivineRight : Bonus
        {
            public DivineRight()
            {
                Name = "Divine Right";
                Text = "+1 to default Grace. Add 1 to each die when praying.";
            }

//            public override void Activate()
//            {
//                Hero.DefaultGrace++;
//                var bonus = new RollBonus(RollType.Pray, BonusType.Results, 1, this);
//                Hero.Add(bonus);
//                Stash.Set(bonus);
//            }
//
//            public override void Deactivate()
//            {
//                base.Deactivate();
//                Hero.DefaultGrace--;
//                var bonus = Stash.Get<RollBonus>();
//                Hero.Remove(bonus);
//            }
        }
        class Inspire : ActionPower
        {
            public Inspire()
            {
                Name = "Inspire";
                StartingPower = true;
                Text = "Activate on a hero in your location.";
                ActiveText = "Deactivate before any die roll for +3d.";
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

//            public override void Activate()
//            {
//                var bonus = new RollBonus(RollType.Elude, BonusType.Dice, 1, this);
//                Hero.Add(bonus);
//                Stash.Set(bonus);
//            }
//
//            public override void Deactivate()
//            {
//                var bonus = Stash.Get<RollBonus>();
//                Hero.Remove(bonus);
//            }
        }

        class Rebellion : TacticPower
        {
            public Rebellion()
                : base(TacticType.Fight, 3)
            {
                Name = "Rebellion";
                Text = "Fight with 3d when attacking a blight or the Necromancer";
            }

            public override bool IsUsable(Hero hero)
            {
                return base.IsUsable(hero) && (hero.State == HeroState.AttackingBlight || hero.State == HeroState.AttackingNecromancer);
            }

//            public override void Activate()
//            {
//                Hero.SetDice(RollType.Fight, 3);
//            }
        }

        class Resistance : ActionPower
        {
            public Resistance()
            {
                Name = "Resistance";
                StartingPower = true;
                Text = "Spend 1 Secrecy to activate in your location.";
                ActiveText = "Heroes gain +1d in fights when attacking blights there.";
            }

            public override bool IsUsable(Hero hero)
            {
                return base.IsUsable(hero) && hero.Secrecy > 0;
            }

//            public override void Activate()
//            {
//                Hero.LoseSecrecy();
//                var space = Hero.GetSpace();
//                var bonus = new RollBonus(RollType.Fight, BonusType.Dice, 1, this) { Restriction = hero => hero.State == HeroState.AttackingBlight };
//                space.Add(bonus);
//                Stash.Add(space, bonus);
//            }
//
//            public override void Deactivate()
//            {
//                base.Deactivate();
//                var space = Stash.Get<ISpace>();
//                var bonus = Stash.Get<RollBonus>();
//                space.Remove(bonus);
//            }
        }
        class SafeHouse : ActionPower
        {
            public SafeHouse()
            {
                Name = "Safe House";
                Text = "Spend 2 Secrecy to activate in your location.";
                ActiveText = "Heroes gain 1 Secrecy (up to 5) when ending a turn there, and +1d when eluding there.";
            }

        }
        class Scouts : ActionPower
        {
            public Scouts()
            {
                Name = "Scouts";
                Text = "Spend 1 Secrecy to activate in your location.";
                ActiveText = "Heroes gain +1d in searches there.";
            }

            public override bool IsUsable(Hero hero)
            {
                return base.IsUsable(hero) && hero.Secrecy > 0;
            }

//            public override void Activate()
//            {
//                var space = Hero.GetSpace();
//                var bonus = new RollBonus(RollType.Search, BonusType.Dice, 1, this);
//                Stash.Add(space, bonus);
//                space.Add(bonus);
//                Hero.LoseSecrecy();
//            }
//
//            public override void Deactivate()
//            {
//                var space = Stash.Get<ISpace>();
//                var bonus = Stash.Get<RollBonus>();
//                space.Remove(bonus);
//            }
        }
        class SecretPassage : ActionPower
        {
            public SecretPassage()
            {
                Name = "Secret Passage";
                Text = "Move to an adjacent location and gain 2 Secrecy (up to 5).";
            }

        }
        class Strategy : TacticPower
        {
            public Strategy()
                : base(TacticType.Fight, 2)
            {
                Name = "Strategy";
                StartingPower = true;
                Text = "Fight with 2d.";
            }

            public override bool IsUsable(Hero hero)
            {
                return base.IsUsable(hero) && (hero.State == HeroState.Fighting || hero.State == HeroState.AttackingBlight || hero.State == HeroState.AttackingNecromancer);
            }

//            public override void Activate()
//            {
//                Hero.SetDice(RollType.Fight, 2);
//            }
        }

        #endregion

    }
}
