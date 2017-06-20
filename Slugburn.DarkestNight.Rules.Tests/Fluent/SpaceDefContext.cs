using Slugburn.DarkestNight.Rules.Blights;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent
{
    public class SpaceDefContext
    {
        private readonly ISpace _space;

        public SpaceDefContext(ISpace space)
        {
            _space = space;
        }

        public SpaceDefContext Blight(params Blight[] blights)
        {
            foreach (var blight in blights)
                _space.AddBlight(blight);
            return this;
        }
    }
}