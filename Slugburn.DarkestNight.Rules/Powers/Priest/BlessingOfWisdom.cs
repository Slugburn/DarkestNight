using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Rolls;

namespace Slugburn.DarkestNight.Rules.Powers.Priest
{
    class BlessingOfWisdom : Blessing
    {
        public BlessingOfWisdom()
        {
            Name = "Blessing of Wisdom";
            Text = "Activate on a hero in your location.";
            ActiveText = "+1d when eluding.";
        }

        public override void HandleCallback(Hero hero, string path, object data)
        {
            var selectedHero = (Hero)data;
            selectedHero.AddRollModifier(new PowerRollBonus(this, RollType.Elude, 1));
        }
    }
}