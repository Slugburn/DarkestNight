using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Modifiers;
using Slugburn.DarkestNight.Rules.Rolls;

namespace Slugburn.DarkestNight.Rules.Powers
{
    public interface IBonusPower : IPower
    {
    }

    public abstract class BonusPower : Power, IBonusPower
    {
        protected BonusPower()
        {
            Type=PowerType.Bonus;
        }

        public override bool IsUsable(Hero hero)
        {
            // Corruption blight prevents use of bonus powers
            return base.IsUsable(hero) && !hero.IsAffectedByBlight(BlightType.Corruption);
        }

        protected void AddModifier(ModifierType modifierType, int amount)
        {
            Owner.AddModifier(new StaticBonusPowerModifer(this, modifierType, amount));
        }

        protected void AddModifier(BonusPowerModifer modifer)
        {
            Owner.AddModifier(modifer);
        }
    }
}