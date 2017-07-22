using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Powers.Scholar
{
    internal class AncientCharm : ActivateablePower
    {
        public AncientCharm()
        {
            Name = "Ancient Charm";
            Text = "Activate in your location.";
            ActiveText = "When a hero has an event there, draw an extra card and discard 1.";
        }

        public override void Activate(Hero hero)
        {
            base.Activate(hero);
            hero.Space.AddEffect(Name, ActiveText);
        }
    }
}