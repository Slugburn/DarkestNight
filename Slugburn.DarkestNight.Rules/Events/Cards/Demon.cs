using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Events.Cards
{
    public class Demon : IEventCard
    {
        public string Name => "Demon";
        public EventDetail Detail => EventDetail.Create(x => x.Text("Compare to Secrecy", "6+: Flying", "4-5: Fearful", "0-3: Deadly").Option("cont", "Continue"));

        public void Resolve(Hero hero, string option)
        {
            var enemy = GetEnemy(hero.Secrecy);
            hero.FaceEnemy(enemy);
        }

        private static string GetEnemy(int secrecy)
        {
            if (secrecy >= 5)
                return "Flying Demon";
            if (secrecy >= 3)
                return "Fearful Demon";
            return "Deadly Demon";
        }
    }
}