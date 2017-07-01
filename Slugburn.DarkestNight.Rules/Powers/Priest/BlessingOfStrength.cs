using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Modifiers;
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
            selectedHero.AddModifier(new PowerRollBonus(this, ModifierType.FightDice, 1));
        }
    }
}