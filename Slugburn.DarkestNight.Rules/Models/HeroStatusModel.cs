using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Models
{
    public class HeroStatusModel
    {
        public static HeroStatusModel FromHero(Hero hero)
        {
            return new HeroStatusModel
            {
                Grace = new HeroValueModel { Value = hero.Grace, Default = hero.DefaultGrace },
                Secrecy = new HeroValueModel { Value = hero.Secrecy, Default = hero.DefaultSecrecy },
                Location = hero.Location.ToString(),
            };
        }

        public string Location { get; set; }

        public HeroValueModel Secrecy { get; set; }

        public HeroValueModel Grace { get; set; }
    }
}