using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Tactics;

namespace Slugburn.DarkestNight.Rules.Enemies
{
    public class Cultist : Enemy
    {
        public Cultist()
        {
            Name = "Cultist";
            Fight = 5;
            Elude = 3;
        }

        public override void Win(Hero hero)
        {
            if (hero.ConflictState.SelectedTactic.Type == TacticType.Fight)
                hero.Game.DecreaseDarkness();
        }

        public override string OutcomeDescription(bool isWin, TacticType tacticType)
        {
            if (!isWin) return "Wound";
            return tacticType == TacticType.Fight ? "-1 Darkness" : null;
        }
    }
}