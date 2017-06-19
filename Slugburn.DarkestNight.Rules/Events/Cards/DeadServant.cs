using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Events.Cards
{
    public class DeadServant : IEventCard
    {
        public string Name => "Dead Servant";
        public EventDetail Detail => EventDetail.Create(x => x.Text("Compare to Secrecy", "5+: Scout", "3-4: Archer", "0-2: Dread").Option("cont", "Continue"));

        public void Resolve(Hero hero, string option)
        {
            var enemy = GetEnemy(hero.Secrecy);
            hero.FaceEnemy(enemy);
        }

        private static string GetEnemy(int secrecy)
        {
            if (secrecy >= 5)
                return "Scout";
            if (secrecy  >2)
                return "Archer";
            return "Dread";
        }
    }
}