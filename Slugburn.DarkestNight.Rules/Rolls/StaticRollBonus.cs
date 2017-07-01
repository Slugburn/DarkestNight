using System.Collections.Generic;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Modifiers;

namespace Slugburn.DarkestNight.Rules.Rolls
{
    public class StaticRollBonus : IModifier
    {
        public static StaticRollBonus Create(string name, ModifierType modifierType, int dieCount)
        {
            return new StaticRollBonus {Name = name, DieCount = dieCount, ModifierTypes = new List<ModifierType> {modifierType}};
        }

        public static StaticRollBonus AnyRoll(string name, int dieCount)
        {
            var modifierTypes = new List<ModifierType>
            {
                ModifierType.FightDice,
                ModifierType.EludeDice,
                ModifierType.SearchDice,
                ModifierType.PrayDice,
                ModifierType.EventDice
            };
            return new StaticRollBonus { Name = name, DieCount = dieCount, ModifierTypes = modifierTypes };
        }

        private StaticRollBonus()
        {
        }

        public string Name { get; set; }

        private int DieCount { get; set; }
        private List<ModifierType> ModifierTypes { get; set; }

        public int GetModifier(Hero hero, ModifierType modifierType)
        {
            return ModifierTypes.Contains(modifierType) ?  DieCount : 0;
        }
    }
}