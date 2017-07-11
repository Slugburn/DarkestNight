using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Tactics;

namespace Slugburn.DarkestNight.Rules.Enemies
{
    public class ScoutingEnemy : Enemy
    {
        private readonly int _secrecyLoss;

        public ScoutingEnemy(string name, int? fight, int elude, int secrecyLoss)
        {
            Name = name;
            Fight = fight;
            Elude = elude;
            _secrecyLoss = secrecyLoss;
        }

        public override void Failure(Hero hero)
        {
            hero.LoseSecrecy(_secrecyLoss, "Enemy");
        }

        public override string OutcomeDescription(bool isWin, TacticType tacticType)
        {
            return !isWin ? $"Lose {_secrecyLoss} Secrecy" : null;
        }
    }
}