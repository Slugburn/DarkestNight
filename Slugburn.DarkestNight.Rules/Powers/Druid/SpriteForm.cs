using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Powers.Druid
{
    class SpriteForm : DruidFormPower
    {
        private const string PowerName = "Sprite Form";

        public SpriteForm()
        {
            Name = PowerName;
            StartingPower = true;
            Text = "Deactivate all Forms. Optionally activate.";
            ActiveText = "Ignore blights' effects unless the Necromancer is present. You cannot gain Grace.";
        }

        public override void Activate(Hero hero)
        {
            base.Activate(hero);
            hero.Game.AddIgnoreBlight(new SpriteFormIgnoreBlight { HeroName = hero.Name });
        }

        private class SpriteFormIgnoreBlight : IIgnoreBlight
        {
            public string Name => PowerName;
            public string HeroName { get; set; }

            public bool IsIgnoring(Hero hero, BlightType blight)
            {
                return hero.Name == HeroName && hero.Location != hero.Game.Necromancer.Location;
            }
        }

        public override bool Deactivate(Hero hero)
        {
            if (!base.Deactivate(hero)) return false;
            hero.Game.RemoveIgnoreBlight(Name);
            return true;
        }
    }
}