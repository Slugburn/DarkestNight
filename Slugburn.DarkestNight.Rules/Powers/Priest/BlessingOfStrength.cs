using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Rolls;

namespace Slugburn.DarkestNight.Rules.Powers.Priest
{
    class BlessingOfStrength : Blessing
    {
        public BlessingOfStrength()
        {
            Name = "Blessing of Strength";
            Text = "Activate on a hero in your location.";
            ActiveText = "+1d in fights.";
        }

        public override void HandleCallback(Hero hero, string path, object data)
        {
            var selectedHero = (Hero) data;
            selectedHero.AddRollModifier(new PowerRollBonus(this, RollType.Fight, 1));
        }
    }
}