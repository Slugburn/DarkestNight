using System.Diagnostics.Eventing.Reader;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Tactics;

namespace Slugburn.DarkestNight.Rules.Enemies
{
    public class Enemy : IEnemy
    {
        public static Enemy Create(string name, int fight, int elude)
        {
            return new Enemy {Name = name, Fight = fight, Elude = elude};
        }

        public int Fight { get; set; }
        public string Name { get; set; }
        public int Elude { get; set; }

        public virtual void Win(Hero hero)
        {
            // nothing
        }

        public virtual void Failure(Hero hero)
        {
            hero.TakeWound();
        }

        public virtual string OutcomeDescription(bool isWin, TacticType tacticType)
        {
            return !isWin ? "Wound." : null;
        }
    }
}