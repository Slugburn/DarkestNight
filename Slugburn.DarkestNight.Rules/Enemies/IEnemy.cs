using System.Collections.Generic;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Enemies
{
    public interface IEnemy
    {
        string Name { get; }
        int Fight { get; }
        int Elude { get; }
        void Win(Hero hero);
        void Failure(Hero hero);
        IEnumerable<ConflictResult> GetResults();
    }
}
