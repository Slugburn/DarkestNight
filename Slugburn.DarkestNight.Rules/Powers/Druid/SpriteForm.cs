using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Powers.Druid
{
    class SpriteForm : DruidFormPower, IBlightSupression
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
            hero.Game.AddBlightSupression(this);
        }

        public override bool Deactivate(Hero hero)
        {
            if (!base.Deactivate(hero)) return false;
            hero.Game.RemoveBlightSupression(Name);
            return true;
        }

        public bool IsSupressed(IBlight blight, Hero hero = null)
        {
            if (hero == null) return false;
            return Owner == hero && hero.Location != hero.Game.Necromancer.Location;
        }
    }
}