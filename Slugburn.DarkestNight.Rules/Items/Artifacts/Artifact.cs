using System.Collections.Generic;

namespace Slugburn.DarkestNight.Rules.Items.Artifacts
{
    public abstract class Artifact : Item, IArtifact
    {
        public static IEnumerable<string> CreateDeck()
        {
            return new[]
            {
                "Blood Ring",
                "Crystal Ball",
                "Ghost Mail",
                "Magic Mask",
                "Starry Veil",
                "Vanishing Cowl"
            };
        }

        protected Artifact(string name) : base(name)
        {
        }
    }
}
