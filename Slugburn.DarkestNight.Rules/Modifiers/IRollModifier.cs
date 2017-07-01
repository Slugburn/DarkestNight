using System.Collections.Generic;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Modifiers
{
    public interface IRollModifier
    {
        ICollection<int> Modify(Hero hero, ModifierType modifierType, ICollection<int> roll);
    }
}
