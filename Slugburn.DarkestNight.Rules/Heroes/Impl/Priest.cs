using Slugburn.DarkestNight.Rules.Powers;
using Slugburn.DarkestNight.Rules.Triggers;

namespace Slugburn.DarkestNight.Rules.Heroes.Impl
{
    public class Priest : Hero
    {
        public Priest()
            : base(
                "Priest", 6, 6, new Benediction(), new BlessingOfFaith(), new BlessingOfPiety(), new BlessingOfStrength(), new BlessingOfWisdom(), new Calm(),
                new Censure(), new Intercession(), new Miracle(), new Sanctuary())
        {
        }

        #region Powers

        class Benediction : ActionPower
        {
            public Benediction()
            {
                Name = "Benediction";
                StartingPower = true;
                Text = "One hero at your location gains 1 Grace (up to default). If they now have more Grace than you, you gain 1 Grace.";
            }
        }

        class BlessingOfFaith : ActionPower
        {
            public BlessingOfFaith()
            {
                Name = "Blessing of Faith";
                StartingPower = true;
                Text = "Activate on a hero in your location.";
                ActiveText = "Gain an extra Grace (up to default) when praying.";
            }
        }

        class BlessingOfPiety : ActionPower
        {
            public BlessingOfPiety()
            {
                Name = "Blessing of Piety";
                Text = "Activate on a hero in your location.";
                ActiveText = "Gain 1 Grace (up to default) when hiding.";
            }
        }

        class BlessingOfStrength : ActionPower
        {
            public BlessingOfStrength()
            {
                Name = "Blessing of Strength";
                Text = "Activate on a hero in your location.";
                ActiveText = "+1d in fights.";
            }

        }

        class BlessingOfWisdom : ActionPower
        {
            public BlessingOfWisdom()
            {
                Name = "Blessing of Wisdom";
                Text = "Activate on a hero in your location.";
                ActiveText = "+1d when eluding.";
            }
        }

        class Calm : Bonus
        {
            public Calm()
            {
                Name = "Calm";
                Text = "Heroes at your location may pray.";
            }

//            public override void Activate()
//            {
//                var space = Hero.GetSpace();
//                var action = new LocationAction(ActionType.Pray, this);
//                Stash.Set(space, action);
//                space.Add(action);
//            }
//
//            public override void Deactivate()
//            {
//                base.Deactivate();
//                var space = Stash.Get<ISpace>();
//                var action = Stash.Get<LocationAction>();
//                space.Remove(action);
//            }
        }

        class Censure : TacticPower
        {
            public Censure()
                : base(TacticType.Fight,2)
            {
                Name = "Censure";
                Text = "Fight with 2d.";
            }

//            public override void Activate()
//            {
//                Hero.SetDice(RollType.Fight, 2);
//            }
        }

        class Intercession : Bonus
        {
            public Intercession()
            {
                Name = "Intercession";
                StartingPower = true;
                Text = "Whenever a hero at your location loses or spends Grace, they may spend your Grace instead.";
            }
        }

        class Miracle : Bonus
        {
            public Miracle()
            {
                Name = "Miracle";
                Text = "Spend 1 Grace to reroll any die roll you make. You may do this repeatedly.";
            }

//            public override void Activate()
//            {
//                var action = new TriggeredAction(Trigger.AfterRoll, Text,
//                    hero => hero.OfferReroll().OnReroll(x => hero.LoseGrace())) { Condition = hero => hero.Grace > 0 };
//                Hero.Add(action);
//                Stash.Set(action);
//            }
//
//            public override void Deactivate()
//            {
//                base.Deactivate();
//                var action = Stash.Get<TriggeredAction>();
//                Hero.Remove(action);
//            }
        }

        class Sanctuary : TacticPower
        {
            public Sanctuary()
                : base(TacticType.Elude, 4)
            {
                Name = "Sanctuary";
                StartingPower = true;
                Text = "Elude with 4d. Lose 1 Secrecy if you succeed.";
            }

            public override bool IsUsable(Hero hero)
            {
                return base.IsUsable(hero) && hero.State == HeroState.Eluding;
            }

//            public override void Activate()
//            {
//                Hero.SetDice(RollType.Elude, 4);
//            }
        }
        #endregion
    }
}
