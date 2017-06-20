using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Events.Cards
{
    public class Patrols : IEventCard
    {
        public string Name => "Patrols";
        public int Fate { get; }

        public EventDetail Detail => EventDetail.Create(x => x.Text("Compare to Darkness",
            "0-14: Archer (Fight: 4, Elude: 4)",
            "15-24: Lich (Fight: 5, Elude: 5)",
            "25+: Reaper (Fight: 6, Elude: 6)")
            .Option("cont", "Continue"));

        public void Resolve(Hero hero, string option)
        {
            var enemy = GetEnemy(hero.Game.Darkness);
            hero.FaceEnemy(enemy);
        }

        private static string GetEnemy(int darkness)
        {
            if (darkness < 15)
                return "Archer";
            if (darkness < 25)
                return "Lich";
            return "Reaper";
        }
    }
}