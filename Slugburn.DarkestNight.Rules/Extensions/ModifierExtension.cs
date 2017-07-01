using System;
using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Blights.Implementations;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Modifiers;

namespace Slugburn.DarkestNight.Rules
{
    public static class ModifierExtension
    {
        public static ModifierSummary CreateModifierSummary(this Hero hero, ModifierType modifierType, string baseName, int baseAmount)
        {
            var details = new List<ModifierDetail> {ModifierDetail.Create(baseName, baseAmount)};
            var otherDetails = (from rollMod in hero.GetModifiers()
                let mod = rollMod.GetModifier(hero, modifierType)
                where mod != 0
                select new ModifierDetail {Name = rollMod.Name, Modifier = mod});
            details.AddRange(otherDetails);
            if (modifierType == ModifierType.FightDice)
                details.AddRange(CreateBlightModifierDetails<UnholyAura>(hero));
            if (modifierType == ModifierType.EludeDice)
                details.AddRange(CreateBlightModifierDetails<EvilPresence>(hero));
            var total = details.Sum(x => x.Modifier);
            total = Math.Max(total, 1);
            var summary = new ModifierSummary(details, total);
            return summary;
        }

        public static int GetModifiedTotal(this Hero hero, int baseAmount, ModifierType modifierType)
        {
            return hero.CreateModifierSummary(modifierType, null, baseAmount).Total;
        }

        private static IEnumerable<ModifierDetail> CreateBlightModifierDetails<T>(Hero hero) where T:IBlight
        {
            return hero.GetActiveBlights<T>().Select(x => ModifierDetail.Create(x.Name, -1));
        }

    }
}
