using System;
using Slugburn.DarkestNight.Rules.Enemies;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Events.Cards
{
    public class BlackBanner : IEventCard
    {
        public string Name => "Black Banner";

        public EventDetail Detail => EventDetail.Create(x => x
            .Text("Count the blights in your location")
            .Text("0-1: Archer")
            .Text("2-3: Lich")
            .Text("4: Reaper")
            .Option("cont", "Continue"));

        public void Resolve(Hero hero, string option)
        {
            if (option != "cont") return;
            var enemy = GetEnemy(hero.GetBlights().Count);
            hero.FaceEnemy(enemy);
        }

        private static string GetEnemy(int count)
        {
            if (count == 0 || count == 1)
                return "Archer";
            if (count == 2 || count == 3)
                return "Lich";
            if (count == 4)
                return "Reaper";
            throw new ArgumentOutOfRangeException(nameof(count));
        }
    }
}
