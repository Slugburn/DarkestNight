using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Players.Models
{
    public class PlayerHeroStatus
    {
        public static PlayerHeroStatus FromHero(Hero hero)
        {
            return new PlayerHeroStatus
            {
                Grace = new PlayerHeroValue { Value = hero.Grace, Default = hero.DefaultGrace },
                Secrecy = new PlayerHeroValue { Value = hero.Secrecy, Default = hero.DefaultSecrecy },
                Location = hero.Location.ToString(),
            };
        }

        public string Location { get; set; }

        public PlayerHeroValue Secrecy { get; set; }

        public PlayerHeroValue Grace { get; set; }
    }
}