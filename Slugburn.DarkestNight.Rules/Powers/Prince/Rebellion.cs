using System.Linq;
using Slugburn.DarkestNight.Rules.Conflicts;
using Slugburn.DarkestNight.Rules.Enemies;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Tactics;

namespace Slugburn.DarkestNight.Rules.Powers.Prince
{
    internal class Rebellion : TacticPower
    {
        public Rebellion()
        {
            Name = "Rebellion";
            Text = "Fight with 3d when attacking a blight or the Necromancer";
        }

        public override bool IsUsable(Hero hero)
        {
            if (!base.IsUsable(hero)) return false;
            if (hero.Enemies.Any(e => e is Necromancer)) return true;
            return hero.ConflictState?.ConflictType == ConflictType.Attack;
        }

        protected override void OnLearn()
        {
            Owner.AddTactic(new PowerTactic(this, TacticType.Fight, 3));
        }
    }
}