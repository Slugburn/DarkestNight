using System;
using System.Linq;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Modifiers;

namespace Slugburn.DarkestNight.Rules
{
    public static class ModifierExtension
    {
        public static ModifierSummary CreateModifierSummary(this Hero hero, ModifierType modifierType, string baseName, int baseAmount)
        {
            var baseDetail = new ModifierDetail {Name = baseName, Modifier = baseAmount};
            var otherDetails = from rollMod in hero.GetRollModifiers()
                let mod = rollMod.GetModifier(hero, modifierType)
                where mod != 0
                select new ModifierDetail {Name = rollMod.Name, Modifier = mod};
            var details = new[] {baseDetail}.Concat(otherDetails).ToList();
            var total = details.Sum(x => x.Modifier);
            total = Math.Max(total, 1);
            var summary = new ModifierSummary(details, total);
            return summary;
        }

        public static int GetModifiedTotal(this Hero hero, int baseAmount, ModifierType modifierType)
        {
            var rollModifiers = hero.GetRollModifiers().ToList();
            var activeMods = from rollMod in rollModifiers
                             let mod = rollMod.GetModifier(hero, modifierType)
                             where mod != 0
                             select mod;
            var modification = activeMods.Sum(m => m);
            var total = modification + baseAmount;
            total = Math.Max(total, 1);
            return total;
        }
    }
}
