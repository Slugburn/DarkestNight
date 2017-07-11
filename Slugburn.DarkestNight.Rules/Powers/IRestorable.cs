using Slugburn.DarkestNight.Rules.IO;

namespace Slugburn.DarkestNight.Rules.Powers
{
    public interface IRestorable
    {
        void Save(PowerData data);
        void Restore(PowerData data);
    }
}
