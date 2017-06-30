using Slugburn.DarkestNight.Rules.Heroes;
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

        public override void Learn(Hero hero)
        {
            base.Learn(hero);
            hero.AddRollModifier(new FadeToBlackRollModifer());
        }

        private class FadeToBlackRollModifer : IRollModifier
        {
            public string Name => PowerName;

            public int GetModifier(Hero hero, RollType rollType)
            {
                if (rollType != RollType.Fight) return 0;
                var power = hero.GetPower(PowerName);
                if (!power.IsUsable(hero)) return 0;
                var game = hero.Game;
                if (game.Darkness < 10) return 0;
                if (game.Darkness < 20) return 1;
                return 2;
            }

        }
    }
}