using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Models
{
    public class HeroStatusModel
    {
        public static HeroStatusModel FromHero(Hero hero)
        {
            return new HeroStatusModel
            {
                Grace = new HeroValueModel("Grace", hero.Grace, hero.DefaultGrace),
                Secrecy = new HeroValueModel("Secrecy", hero.Secrecy, hero.DefaultSecrecy),
                Location = hero.Location.ToString(),
            };
        }

        public string Location { get; set; }

        public HeroValueModel Secrecy { get; set; }

        public HeroValueModel Grace { get; set; }
    }
}