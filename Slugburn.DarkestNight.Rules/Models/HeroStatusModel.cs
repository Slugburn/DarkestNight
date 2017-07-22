using System.Linq;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Models
{
    public class HeroStatusModel
    {
        public static HeroStatusModel FromHero(Hero hero)
        {
            return new HeroStatusModel
            {
                Location = hero.Location.ToString(),
                Grace = new HeroValueModel("Grace", hero.Grace, hero.DefaultGrace),
                Secrecy = new HeroValueModel("Secrecy", hero.Secrecy, hero.DefaultSecrecy),
                HasTakenTurn = hero.HasTakenTurn,
                CanStartTurn = hero.AvailableCommands != null && hero.AvailableCommands.Any(x => x.Name == "Start Turn")
            };
        }

        public string Location { get; set; }
        public HeroValueModel Grace { get; set; }
        public HeroValueModel Secrecy { get; set; }
        public bool CanStartTurn { get; set; }
        public bool HasTakenTurn { get; set; }
    }
}