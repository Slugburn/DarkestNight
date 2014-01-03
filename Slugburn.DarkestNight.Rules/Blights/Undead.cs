using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Blights
{
    public class Undead : BlightImpl
    {
        public int FightTarget { get; set; }
        public int EvadeTarget { get; set; }

        public Undead(string name, int might, int fightTarget, int evadeTarget)
        {
            Name = name;
            Might = might;
            FightTarget = fightTarget;
            EvadeTarget = evadeTarget;
        }

        public override void Defend(IHero hero)
        {
            hero.TakeWound();
        }
    }
}
