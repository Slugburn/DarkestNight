using System.Linq;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Extensions;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent
{
    public class SpaceDefContext
    {
        private readonly ISpace _space;

        public SpaceDefContext(ISpace space)
        {
            _space = space;
        }

        public SpaceDefContext Blight(params string[] blights)
        {
            foreach (var blight in blights.Select(b=>b.ToEnum<Blight>()))
                _space.AddBlight(blight);
            return this;
        }
    }
}