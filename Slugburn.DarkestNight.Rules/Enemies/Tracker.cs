using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Tactics;

namespace Slugburn.DarkestNight.Rules.Enemies
{
    public class Tracker : Enemy
    {
        public Tracker()
        {
            Name = "Tracker";
            Fight = 4;
            Elude = 5;
        }

        public override void Win(Hero hero)
        {
            var tacticType = hero.ConflictState.SelectedTactic.Type;
            if (tacticType == TacticType.Fight)
                hero.LoseSecrecy("Enemy");
        }

        public override void Failure(Hero hero)
        {
            hero.LoseSecrecy(2, "Enemy");
        }

        public override string OutcomeDescription(bool isWin, TacticType tacticType)
        {
            if (!isWin) return "Lose 2 Secrecy";
            return tacticType == TacticType.Fight
                ? "Lose 1 Secrecy"
                : null;
        }
    }
}
