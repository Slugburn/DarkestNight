using System.Linq;

namespace Slugburn.DarkestNight.Rules.Blights
{
    public static class BlightExtension
    {
        private static readonly Blight[] UndeadList = {Blight.Lich, Blight.Shades, Blight.Skeletons, Blight.Vampire, Blight.Zombies};

        public static bool IsUndead(this Blight blight)
        {
            return UndeadList.Contains(blight);
        }
    }
}
