using System.Collections.Generic;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules
{
    public interface IBlightSelectedHandler
    {
        void Handle(Hero hero, IEnumerable<BlightLocation> selection);
    }
}