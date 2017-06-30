using System.Collections.Generic;

namespace Slugburn.DarkestNight.Rules.Heroes
{
    internal class ActionFilter
    {
        public string Name { get; set; }
        public ICollection<string> Allowed { get; set; }
    }
}