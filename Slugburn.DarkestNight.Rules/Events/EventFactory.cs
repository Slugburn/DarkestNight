using System;
using Slugburn.DarkestNight.Rules.Events.Cards;
using Slugburn.DarkestNight.Rules.Events.Cards.Enemies;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Spaces;

namespace Slugburn.DarkestNight.Rules.Events
{
    public static class EventFactory
    {
        public static string[] GetEventDeck()
        {
            return new[]
            {
                "Altar", "Anathema", "Betrayal", "Black Banner", "Close Call", "Cultist", "Dark Champion", "Dark Scrying",
                "Dead Servant", "Dead Servant", "Dead Servant", "Demon", "Demon", "Evil Day", "Guarded Trove",
                "Horde", "Horde", "Horde", "Latent Spell", "Lich", "Looters", "Midnight", "Patrols", "Raid", "Renewal",
                "Ritual", "Shambling Horror", "Sloppy Search", "Tracker", "Twist of Fate", "Unfriendly Eyes", "Upheaval",
                "Vengeful Spirit", "Vile Messenger"
            };
        } 

        public static IEventCard CreateCard(string name)
        {
            switch (name)
            {
                case "Altar":
                    return new Altar();
                case "Anathema":
                    return new EventCard("Anathema", 6, "Lose 1 Grace.", hero => hero.LoseGrace());
                case "Betrayal":
                    return new EventCard("Betrayal",5, "Lose 1 Secrecy.", hero => hero.LoseSecrecy("Event"));
                case "Black Banner":
                    return new BlackBanner();
                case "Close Call":
                    return new CloseCall();
                case "Cultist":
                    return new SingleEnemyEventCard(name,1);
                case "Dark Champion":
                    return new DarkChampion();
                case "Dark Scrying":
                    return new DarkScrying();
                case "Dead Servant":
                    return new DeadServant();
                case "Demon":
                    return new Demon();
                case "Evil Day":
                    return new EvilDay();
                case "Guarded Trove":
                    return new SingleEnemyEventCard(name, 1);
                case "Horde":
                    return new Horde();
                case "Latent Spell":
                    return new LatentSpell();
                case "Lich":
                    return new SingleEnemyEventCard(name, 4);
                case "Looters":
                    return new SingleEnemyEventCard(name,2);
                case "Midnight":
                    return new EventCard(name,7, "+1 Darkness.", hero=>hero.Game.IncreaseDarkness());
                case "Patrols":
                    return new Patrols();
                case "Raid":
                    return new Raid();
                case "Renewal":
                    return new Renewal();
                case "Ritual":
                    return new Ritual();
                case "Shambling Horror":
                    return new ShamblingHorror();
                case "Sloppy Search":
                    return new SloppySearch();
                case "Tracker":
                    return new SingleEnemyEventCard(name, 5);
                case "Twist of Fate":
                    return new TwistOfFate();
                case "Unfriendly Eyes":
                    return new UnfriendlyEyes();
                case "Upheaval":
                    return new EventCard(name,2, "Remove all blights from your current location and create an equal number of new blights", Upheaval);
                case "Vengeful Spirit":
                    return new VengefulSpirit();
                case "Vile Messenger":
                    return new SingleEnemyEventCard(name,4);
                default:
                    throw new ArgumentOutOfRangeException(nameof(name),name, "Unknown event name");
            }
        }

        private static void Upheaval(Hero hero)
        {
            var count = hero.GetBlights().Count;
            var space = (Space) hero.GetSpace();
            foreach (var blight in hero.GetBlights())
                space.RemoveBlight(blight);
            hero.Game.CreateBlights(hero.Location, count);
        }
    }
}
