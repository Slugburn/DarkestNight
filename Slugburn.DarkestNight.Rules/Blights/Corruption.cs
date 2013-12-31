namespace Slugburn.DarkestNight.Rules.Blights
{
    public class Corruption : Blight
    {
        public Corruption() 
        {
            Name = "Corruption";
            Might = 5;
        }

        public override void Defend(IHero hero)
        {
            hero.ExhaustPowers();
        }

    }
}
