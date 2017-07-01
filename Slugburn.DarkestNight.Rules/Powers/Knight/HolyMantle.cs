using System.Collections.Generic;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Modifiers;

namespace Slugburn.DarkestNight.Rules.Powers.Knight
{
    class HolyMantle : BonusPower, IRollModifier
    {
        public HolyMantle()
        {
            Name = "Holy Mantle";
            Text = "+1 to default Grace. Add 1 to each die when praying.";
        }

        protected override void OnLearn()
        {
            AddModifier(ModifierType.DefaultGrace, 1);
            Owner.AddRollModifier(this);
        }

        public ICollection<int> Modify(Hero hero, ModifierType modifierType, ICollection<int> roll)
        {
            return modifierType == ModifierType.PrayDice && IsUsable(hero)
                ? roll.AddOneToEach()
                : roll;
        }
    }
}