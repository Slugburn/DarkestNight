using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Blights.Implementations
{
    public class Undead : BlightBase
    {
        public int FightTarget { get; }
        public int EludeTarget { get; }

        public Undead(Blight type,  string name, int might, int fightTarget, int eludeTarget) : base(type)
        {
            Name = name;
            EffectText = $"At the end of each turn in the affected location, a hero must combat a {name.ToLower()}.";
            Might = might;
            DefenseText = "Wound.";
            FightTarget = fightTarget;
            EludeTarget = eludeTarget;
        }

        public override void Defend(Hero hero)
        {
            hero.TakeWound();
        }
    }
}
