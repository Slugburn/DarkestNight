using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Blights.Implementations
{
    class Spies : Blight, ISource
    {
        public Spies() : base(BlightType.Spies)
        {
            Name = "Spies";
            EffectText = "At the end of each turn in the affected location, a hero loses 1 Secrecy.";
            Might = 5;
            DefenseText = "Lose 1 Secrecy.";
        }

        public override void Failure(Hero hero)
        {
            hero.LoseSecrecy(Name);
        }
    }
}
