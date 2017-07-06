using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Modifiers;
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

        public override void HandleCallback(Hero hero, object data)
        {
            var selectedHero = (Hero)data;
            selectedHero.AddModifier(new PowerRollBonus(this, ModifierType.EludeDice, 1));
        }
    }
}