using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Players.Models
{
    public class PlayerHero
    {
        public string Name { get; set; }

        private PlayerHero(string name)
        {
            Name = name;
        }

        public static PlayerHero FromHero(Hero hero)
        {
            return new PlayerHero(hero.Name);
        }
    }
}