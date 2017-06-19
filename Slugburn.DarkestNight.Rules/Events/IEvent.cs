using System.Security.Cryptography.X509Certificates;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Events
{
    public interface IEvent
    {
        string Name { get; }
        EventDetail Detail { get; }
        void Resolve(Hero hero, string option);
    }
}