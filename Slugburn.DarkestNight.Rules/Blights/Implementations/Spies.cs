using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Blights.Implementations
{
    class Spies : BlightBase, ISource
    {
        public Spies() : base(Blight.Spies)
        {
            Name = "Spies";
            EffectText = "At the end of each turn in the affected location, a hero loses 1 Secrecy.";
            Might = 5;
            DefenseText = "Lose 1 Secrecy.";
        }

        public override void Defend(Hero hero)
        {
            hero.LoseSecrecy(Name);
        }
    }
}
