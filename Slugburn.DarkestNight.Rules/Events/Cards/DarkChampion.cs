using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Events.Cards
{
    public class DarkChampion : IEventCard
    {
        public string Name => "Dark Champion";
        public EventDetail Detail => EventDetail.Create(x => x.Text("Compare to Darkness", "0-9: Ghoul", "10-19: Revenant", "20+: Slayer").Option("cont", "Continue"));

        public void Resolve(Hero hero, string option)
        {
            var enemy = GetEnemy(hero.Game.Darkness);
            hero.FaceEnemy(enemy);
        }

        private static string GetEnemy(int darkness)
        {
            if (darkness < 10)
                return "Ghoul";
            if (darkness < 20)
                return "Revenant";
            return "Slayer";
        }
    }
}