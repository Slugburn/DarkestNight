using System.Collections.Generic;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Modifiers;

namespace Slugburn.DarkestNight.Rules.Items
{
    class HolyRelic : Item, IRollModifier
    {
        public HolyRelic() : base("Holy Relic")
        {
        }

        public ICollection<int> Modify(Hero hero, ModifierType modifierType, ICollection<int> roll)
        {
            if (modifierType != ModifierType.FightDice) return roll;
            return roll.AddOneToHighest();
        }

    }
}
