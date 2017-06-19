using Slugburn.DarkestNight.Rules.Enemies;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Blights.Implementations
{
    public class Undead : BlightDetail
    {
        private readonly string _enemyName;

        public Undead(Blight type,  string name, int might, string enemyName) : base(type)
        {
            _enemyName = enemyName;
            Name = name;
            EffectText = $"At the end of each turn in the affected location, a hero must combat a {enemyName.ToLower()}.";
            Might = might;
            DefenseText = "Wound.";
        }

        public override void Defend(Hero hero)
        {
            hero.TakeWound();
        }

        public IEnemy CreateEnemy()
        {
            return EnemyFactory.Create(_enemyName);
        }
    }
}
