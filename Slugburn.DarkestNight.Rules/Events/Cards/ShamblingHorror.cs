using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Events.Cards
{
    public class ShamblingHorror : IEventCard
    {
        public string Name => "Shambling Horror";
        public int Fate { get; }

        public EventDetail Detail => EventDetail.Create(x => x.Text("Compare to Secrecy",
            "6+: Zombie (Fight: 5, Elude: 3)",
            "4-5: Mummy (Fight: 6, Elude: 4)",
            "0-3: Slayer (Fight: 6, Elude: 5)")
            .Option("cont", "Continue"));

        public void Resolve(Hero hero, string option)
        {
            var enemy = GetEnemy(hero.Secrecy);
            hero.FaceEnemy(enemy);
        }

        private static string GetEnemy(int secrecy)
        {
            if (secrecy >= 6)
                return "Zombie";
            if (secrecy >= 4)
                return "Mummy";
            return "Slayer";
        }
    }
}