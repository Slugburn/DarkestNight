using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Modifiers;
using Slugburn.DarkestNight.Rules.Rolls;

namespace Slugburn.DarkestNight.Rules.Powers.Acolyte
{
    class FadeToBlack : BonusPower
    {
        private const string PowerName = "Fade to Black";

        public FadeToBlack()
        {
            Name = PowerName;
            Text = "+1 die in fights when Darkness is 10 or more. Another +1 die in fights when Darkness is 20 or more.";
        }

        protected override void OnLearn()
        {
            Owner.AddModifier(new FadeToBlackRollModifer(this));
        }

        private class FadeToBlackRollModifer : BonusPowerModifer
        {
            public FadeToBlackRollModifer(IBonusPower power) : base(power, ModifierType.FightDice)
            {
            }

            protected override int GetAmount()
            {
                var game = Power.Owner.Game;
                if (game.Darkness >= 20)
                    return 2;
                return game.Darkness >= 10 ? 1 : 0;
            }
        }
    }
}