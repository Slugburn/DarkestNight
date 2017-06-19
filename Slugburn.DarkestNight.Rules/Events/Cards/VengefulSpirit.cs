using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Events.Cards
{
    public class VengefulSpirit : IEventCard
    {
        public string Name => "Vengeful Spirit";

        public EventDetail Detail => EventDetail.Create(x => x.Text("Compare to Secrecy",
            "5+: Shade (Fight: 3, Elude: 5)",
            "4-5: Shadow (Fight: 4, Elude: 6)",
            "0-3: Hunter (Fight: 5, Elude: 6)"));

        public void Resolve(Hero hero, string option)
        {
            var enemy = GetEnemy(hero.Secrecy);
            hero.FaceEnemy(enemy);
        }

        private static string GetEnemy(int secrecy)
        {
            if (secrecy >= 5)
                return "Shade";
            if (secrecy >= 4)
                return "Shadow";
            return "Hunter";
        }
    }
}