namespace Slugburn.DarkestNight.Rules.Blights
{
    public class EvilPresence : Blight
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
