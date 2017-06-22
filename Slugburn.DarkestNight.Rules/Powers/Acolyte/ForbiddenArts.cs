using Slugburn.DarkestNight.Rules.Actions;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Rolls;

namespace Slugburn.DarkestNight.Rules.Powers.Acolyte
{
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
                var roll = Die.Roll();
                if (roll == 1)
                    hero.Game.IncreaseDarkness();
                hero.CurrentRoll.ActualRoll.Add(roll);
                hero.CurrentRoll.AdjustRoll();
            }
        }
    }
}