using System;
using Slugburn.DarkestNight.Rules.Events.Cards;
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
                    return new EventCard("Anathema", "Lose 1 Grace.", hero => hero.LoseGrace());
                case "Betrayal":
                    return new EventCard("Betrayal", "Lose 1 Secrecy.", hero => hero.LoseSecrecy("Event"));
                case "Black Banner":
                    return new BlackBanner();
                case "CloseCall":
                    return new CloseCall();
                case "Cultist":
                    return new EventCard("Cultist", "Fight: 5, Elude: 3", hero => hero.FaceEnemy(name));
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
                    return new EventCard(name, "Fight: 6, Elude: 6", hero=>hero.FaceEnemy(name));
                case "Horde":
                    return new Horde();
                case "Latent Spell":
                    return new LatentSpell();
                case "Lich":
                    return new EventCard(name, "Fight: 5, Elude: 5", hero => hero.FaceEnemy(name));
                case "Looters":
                    return new EventCard(name, "Elude: 4", hero=>hero.FaceEnemy(name));
                case "Midnight":
                    return new EventCard(name, "+1 Darkness.", hero=>hero.Game.IncreaseDarkness());
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
                    return new EventCard(name, x => x
                        .Text("Fight: 4, Elude: 5", "Win fight: Lose 1 Secrecy", "Win elude: No effect", "Failure: Lose 2 Secrecy)"),
                        (h, o) => h.FaceEnemy(name));
                case "Twist of Fate":
                    return new TwistOfFate();
                case "Unfriendly Eyes":
                    return new UnfriendlyEyes();
                case "Upheaval":
                    return new EventCard(name, "Remove all blights from your current location and create an equal number of new blights", Upheaval);
                case "Vengeful Spirit":
                    return new VengefulSpirit();
                case "Vile Messenger":
                    return new EventCard(name, x => x.Text("Fight: 4, Elude: -", "Failure: +1 Darkness"), (h, o) => h.FaceEnemy(name));
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
