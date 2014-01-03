using Slugburn.DarkestNight.Rules.Powers;

namespace Slugburn.DarkestNight.Rules.Heroes.Impl
{
    public class Priest : Hero
    {
        public Priest()
        {
            Name = "Priest";
            DefaultGrace = 6;
            DefaultSecrecy = 6;
            Powers = new IPower[]
                     {
                         new Benediction(), 
                         new BlessingOfFaith(), 
                         new BlessingOfPiety(), 
                         new BlessingOfStrength(), 
                         new BlessingOfWisdom(), 
                         new Calm(), 
                         new Censure(), 
                         new Intercession(), 
                         new Miracle(), 
                         new Sanctuary(), 
                     };
        }

        #region Powers

        class Benediction : Action
        {
            public Benediction()
            {
                Name = "Benediction";
                StartingPower = true;
                Text = "One hero at your location gains 1 Grace (up to default). If they now have more Grace than you, you gain 1 Grace.";
            }

            public override void Activate()
            {
                Hero.SelectTarget<IHero>(TargetType.Hero)
                    .OnTarget(target =>
                    {
                        target.GainGrace(1, target.DefaultGrace);
                        if (target.Grace > Hero.Grace)
                            Hero.GainGrace(1);
                    });
            }
        }

        class BlessingOfFaith : Action
        {
            public BlessingOfFaith()
            {
                Name = "Blessing of Faith";
                StartingPower = true;
                Text = "Activate on a hero in your location.";
                ActiveText = "Gain an extra Grace (up to default) when praying.";
            }

            public override void Activate()
            {
                Hero.SelectTarget<IHero>(TargetType.Hero)
                    .OnTarget(target =>
                    {
                        var effect = new TriggeredEffect(Trigger.Praying, ActiveText, h => h.GainGrace(1, h.DefaultGrace), this);
                        target.Add(effect);
                        Stash.Set(target, effect);
                    });
            }

            public override void Deactivate()
            {
                base.Deactivate();
                var target = Stash.Get<IHero>();
                var effect = Stash.Get<TriggeredEffect>();
                target.Remove(effect);
            }

            protected override void SuspendEffects(bool isSuspended)
            {
                Stash.Get<TriggeredEffect>().Active = !isSuspended;
            }

        }

        class BlessingOfPiety : Action
        {
            public BlessingOfPiety()
            {
                Name = "Blessing of Piety";
                Text = "Activate on a hero in your location.";
                ActiveText = "Gain 1 Grace (up to default) when hiding.";
            }

            public override void Activate()
            {
                var space = Hero.GetSpace();
                var effect = new TriggeredEffect(Trigger.Hiding, ActiveText, hero => hero.GainGrace(1, hero.DefaultGrace), this);
                space.Add(effect);
                Stash.Set(space, effect);
            }

            public override void Deactivate()
            {
                base.Deactivate();
                var space = Stash.Get<ISpace>();
                var effect = Stash.Get<TriggeredEffect>();
                space.Remove(effect);
            }

            protected override void SuspendEffects(bool isSuspended)
            {
                Stash.Get<TriggeredEffect>().Active = !isSuspended;
            }
        }

        class BlessingOfStrength : Action
        {
            public BlessingOfStrength()
            {
                Name = "Blessing of Strength";
                Text = "Activate on a hero in your location.";
                ActiveText = "+1d in fights.";
            }

            public override void Activate()
            {
                Hero.SelectTarget<IHero>(TargetType.Hero)
                    .OnTarget(target =>
                    {
                        var bonus = new RollBonus(RollType.Fight, BonusType.Dice, 1, this);
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

            protected override void SuspendEffects(bool isSuspended)
            {
                Stash.Get<RollBonus>().Active = !isSuspended;
            }

        }

        class BlessingOfWisdom : Action
        {
            public BlessingOfWisdom()
            {
                Name = "Blessing of Wisdom";
                Text = "Activate on a hero in your location.";
                ActiveText = "+1d when eluding.";
            }

            public override void Activate()
            {
                Hero.SelectTarget<IHero>(TargetType.Hero)
                    .OnTarget(target =>
                    {
                        var bonus = new RollBonus(RollType.Elude, BonusType.Dice, 1, this);
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

            protected override void SuspendEffects(bool isSuspended)
            {
                if (!Active) return;
                var bonus = Stash.Get<RollBonus>();
                bonus.Active = isSuspended;
            }

        }

        class Calm : Bonus
        {
            public Calm()
            {
                Name = "Calm";
                Text = "Heroes at your location may pray.";
            }

            public override void Activate()
            {
                var space = Hero.GetSpace();
                var action = new LocationAction(ActionType.Pray, this);
                Stash.Set(space, action);
                space.Add(action);
            }

            public override void Deactivate()
            {
                base.Deactivate();
                var space = Stash.Get<ISpace>();
                var action = Stash.Get<LocationAction>();
                space.Remove(action);
            }

            protected override void SuspendEffects(bool isSuspended)
            {
                if (!Active) return;
                var action = Stash.Get<LocationAction>();
                action.Active = isSuspended;
            }

        }

        class Censure : Tactic
        {
            public Censure()
                : base(TacticType.Fight)
            {
                Name = "Censure";
                Text = "Fight with 2d.";
            }

            public override void Activate()
            {
                Hero.SetDice(RollType.Fight, 2);
            }
        }

        class Intercession : Bonus
        {
            public Intercession()
            {
                Name = "Intercession";
                StartingPower = true;
                Text = "Whenever a hero at your location loses or spends Grace, they may spend your Grace instead.";
            }

            public override void Activate()
            {
                throw new System.NotImplementedException();
            }
        }

        class Miracle : Bonus
        {
            public Miracle()
            {
                Name = "Miracle";
                Text = "Spend 1 Grace to reroll any die roll you make. You may do this repeatedly.";
            }

            public override void Activate()
            {
                var action = new TriggeredAction(Trigger.AfterRoll, Text,
                    hero => hero.OfferReroll().OnReroll(x => hero.LoseGrace())) { Condition = hero => hero.Grace > 0 };
                Hero.Add(action);
                Stash.Set(action);
            }

            public override void Deactivate()
            {
                base.Deactivate();
                var action = Stash.Get<TriggeredAction>();
                Hero.Remove(action);
            }
        }

        class Sanctuary : Tactic
        {
            public Sanctuary()
                : base(TacticType.Elude)
            {
                Name = "Sanctuary";
                StartingPower = true;
                Text = "Elude with 4d. Lose 1 Secrecy if you succeed.";
            }

            public override bool IsUsable()
            {
                return base.IsUsable() && Hero.State == HeroState.Eluding;
            }

            public override void Activate()
            {
                Hero.SetDice(RollType.Elude, 4);
            }

            public override void OnSuccess()
            {
                Hero.LoseSecrecy();
            }
        }
        #endregion
    }
}
