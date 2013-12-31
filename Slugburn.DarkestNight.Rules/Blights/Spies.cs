namespace Slugburn.DarkestNight.Rules.Blights
{
    class Spies : Blight
    {
        public Spies() 
        {
            Name = "Spies";
            Might = 5;
        }

        public override void Defend(IHero hero)
        {
            hero.LoseSecrecy();
        }
    }
}
