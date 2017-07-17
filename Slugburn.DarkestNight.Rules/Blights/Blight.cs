using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Spaces;
using Slugburn.DarkestNight.Rules.Tactics;
using Slugburn.DarkestNight.Rules.Triggers;

namespace Slugburn.DarkestNight.Rules.Blights
{
    public abstract class Blight : IBlight
    {
        private Space _space;
        private int _might;
        public BlightType Type { get; }

        protected Blight(BlightType type)
        {
            Type = type;
        }

        public int Id { get; set; }
        public string Name { get; protected set; }

        public int Might
        {
            get { return _might + (_space.Game.Darkness >= 25 ? 1 : 0); }
            set { _might = value; }
        }

        public string EffectText { get; protected set; }
        public string DefenseText { get; protected set; }
        public bool IsSupressed
        {
            get
            {
                var game = _space.Game;
                return game.IsBlightSupressed(this, game.ActingHero);
            }
        }

        public Location Location => _space.Location;

        public void Win(Hero hero)
        {
            hero.Game.DestroyBlight(hero, Id);
            hero.Triggers.Send(HeroTrigger.DestroyedBlight);
        }

        public abstract void Failure(Hero hero);

        public void SetSpace(Space space)
        {
            _space = space;
        }

        public string OutcomeDescription(bool isWin, TacticType tacticType)
        {
            return isWin ? "Destroy blight." : DefenseText;
        }
    }
}
