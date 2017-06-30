using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Blights.Implementations
{
    public class DarkFog : Blight
    {
        public DarkFog() :base(BlightType.DarkFog)
        {
            Name = "Dark Fog";
            EffectText = "The search difficulty at the affected location is increased by 2";
            Might = 5;
            DefenseText = "Lose a turn.";
        }

        public override void Failure(Hero hero)
        {
            hero.LoseTurn();
        }
    }
}
