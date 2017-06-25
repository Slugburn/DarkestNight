using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Blights.Implementations
{
    public class EnemyGenerator : BlightDetail
    {
        public string EnemyName { get; }

        public EnemyGenerator(Blight type,  string name, int might, string enemyName) : base(type)
        {
            EnemyName = enemyName;
            Name = name;
            EffectText = $"At the end of each turn in the affected location, a hero must combat a {enemyName.ToLower()}.";
            Might = might;
            DefenseText = "Wound.";
        }

        public override void Failure(Hero hero)
        {
            hero.TakeWound();
        }
    }
}
