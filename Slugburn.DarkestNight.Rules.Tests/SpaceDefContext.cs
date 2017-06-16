using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Spaces;

namespace Slugburn.DarkestNight.Rules.Tests
{
    public class SpaceDefContext
    {
        private readonly ISpace _space;

        public SpaceDefContext(ISpace space)
        {
            _space = space;
        }

        public SpaceDefContext Blight(params BlightType[] blightTypes)
        {
            var factory= new BlightFactory();
            foreach (var blight in blightTypes)
            {
                _space.AddBlight(factory.Create(blight));
            }
            return this;
        }
    }
}