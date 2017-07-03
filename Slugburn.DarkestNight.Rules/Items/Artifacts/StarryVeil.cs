using Slugburn.DarkestNight.Rules.Commands;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Items.Artifacts
{
    class StarryVeil : Artifact, ICommand
    {
        public StarryVeil() : base("Starry Veil")
        {
            Text = "When any hero at your location draws an event with a Fate of 5 or more, "
                   + "they may discard it and draw another.";
        }

        public bool IsAvailable(Hero hero)
        {
            if (hero.State != HeroState.EventDrawn) return false;
            if (hero.Location != Owner.Location) return false;
            if (!hero.CurrentEvent.IsIgnorable) return false;
            return hero.CurrentEvent.Fate >= 5;
        }

        public void Execute(Hero hero)
        {
            hero.EndEvent();
            hero.DrawEvent();
        }
    }
}
