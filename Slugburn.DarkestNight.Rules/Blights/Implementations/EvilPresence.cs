using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Blights.Implementations
{
    public class EvilPresence : Blight
    {
        public EvilPresence() : base(BlightType.EvilPresence)
        {
            Name = "Evil Presence";
            EffectText = "While a hero is in the affected location, he rolls one fewer die when eluding (to a minimum of 1).";
            Might = 4;
            DefenseText = "Event.";
        }

        public override void Defend(Hero hero)
        {
            hero.DrawEvent();
        }
    }
}
