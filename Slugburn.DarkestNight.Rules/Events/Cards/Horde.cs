using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Events.Cards
{
    public class Horde : IEventCard
    {
        public string Name => "Horde";
        public EventDetail Detail => EventDetail.Create(x => x.Text("Compare to Secrecy", "4+: Small", "2-3: Large", "0-1: Giant").Option("cont", "Continue"));

        public void Resolve(Hero hero, string option)
        {
            var enemy = GetEnemy(hero.Secrecy);
            hero.FaceEnemy(enemy);
        }

        private static string GetEnemy(int secrecy)
        {
            if (secrecy >= 4)
                return "Small Horde";
            if (secrecy >= 2)
                return "Large Horde";
            return "Giant Horde";
        }
    }
}