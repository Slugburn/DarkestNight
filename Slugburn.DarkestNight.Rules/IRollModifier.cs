using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules
{
    public interface IRollModifier
    {
        int GetModifier(Hero hero);
        string Name { get; }
    }
}
