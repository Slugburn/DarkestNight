using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Blights
{
    public class EvilPresence : BlightImpl
    {
        public EvilPresence() 
        {
            Name = "Evil Presence";
            Might = 4;
        }

        public override void Defend(IHero hero)
        {
            hero.DrawEvent();
        }
    }
}
