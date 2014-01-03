using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Extensions;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules
{
    public class GameFactory
    {
        private readonly BlightFactory _blightFactory;
        private readonly IEnumerable<IHero> _availableHeroes;

        public GameFactory(
            BlightFactory blightFactory,
            IEnumerable<IHero> availableHeroes)
        {
            _blightFactory = blightFactory;
            _availableHeroes = availableHeroes;
        }

        public Game Create()
        {
            var game = new Game
            {
                Board = new Board(),
                Events = new List<IEvent>(),
                Maps = CreateMaps().Shuffle(),
                Heroes = _availableHeroes.Shuffle().Take(4).ToArray(),
                AvailableHeroes = _availableHeroes,
                Darkness = 0,
            };
            game.Necromancer = new Necromancer(game);

            PopulateInitialBlights(game);

            return game;
        }

        private static IEnumerable<IMap> CreateMaps()
        {
            return new IMap[]
                   {
                       new Map(new[] {Blight.Shades, Blight.Confusion, Blight.UnholyAura, Blight.Corruption, Blight.DarkFog, Blight.Desecration, Blight.Spies},
                           new[] {Find.ForgottenShrine, Find.Key, Find.TreasureChest, Find.Key, Find.VanishingDust, Find.Key}),
                       new Map(new[] {Blight.Shroud, Blight.UnholyAura, Blight.Skeletons, Blight.Confusion, Blight.DarkFog, Blight.EvilPresence, Blight.Spies},
                           new[] {Find.Key, Find.VanishingDust, Find.TreasureChest, Find.Epiphany, Find.TreasureChest, Find.Key}),
                       new Map(new[] {Blight.Desecration, Blight.EvilPresence, Blight.Taint, Blight.Lich, Blight.Corruption, Blight.UnholyAura, Blight.Spies},
                           new[] {Find.TreasureChest, Find.TreasureChest, Find.TreasureChest, Find.Epiphany, Find.Key, Find.ForgottenShrine}),
                       new Map(new[] {Blight.Skeletons, Blight.Shroud, Blight.Corruption, Blight.Shades, Blight.Lich, Blight.DarkFog, Blight.Curse},
                           new[] {Find.SupplyCache, Find.SupplyCache, Find.SupplyCache, Find.BottledMagic, Find.Key, Find.Key}),
                       new Map(new[] {Blight.Desecration, Blight.Vampire, Blight.Confusion, Blight.Skeletons, Blight.Shades, Blight.Lich, Blight.Spies},
                           new[] {Find.SupplyCache, Find.SupplyCache, Find.Key, Find.Epiphany, Find.Waystone, Find.Key}),
                       new Map(new[] {Blight.Lich, Blight.EvilPresence, Blight.Spies, Blight.Confusion, Blight.Skeletons, Blight.Shroud, Blight.Desecration},
                           new[] {Find.Key, Find.Waystone, Find.TreasureChest, Find.BottledMagic, Find.SupplyCache, Find.SupplyCache}),
                       new Map(new[] {Blight.Zombies, Blight.Spies, Blight.Lich, Blight.Desecration, Blight.Shroud, Blight.EvilPresence, Blight.Taint},
                           new[] {Find.TreasureChest, Find.BottledMagic, Find.Key, Find.Artifact, Find.Artifact, Find.Key}),
                       new Map(new[] {Blight.Shroud, Blight.Skeletons, Blight.EvilPresence, Blight.Shades, Blight.UnholyAura, Blight.Corruption, Blight.Taint},
                           new[] {Find.Key, Find.SupplyCache, Find.SupplyCache, Find.Epiphany, Find.Key, Find.SupplyCache}),
                       new Map(new[] {Blight.Curse, Blight.UnholyAura, Blight.Spies, Blight.Skeletons, Blight.Confusion, Blight.Vampire, Blight.Desecration},
                           new[] {Find.TreasureChest, Find.ForgottenShrine, Find.SupplyCache, Find.Epiphany, Find.TreasureChest, Find.Key}),
                       new Map(new[] {Blight.Shades, Blight.DarkFog, Blight.Zombies, Blight.Curse, Blight.Desecration, Blight.Spies, Blight.UnholyAura},
                           new[] {Find.Key, Find.TreasureChest, Find.TreasureChest, Find.BottledMagic, Find.Key, Find.Key}),
                       new Map(new[] {Blight.EvilPresence, Blight.Skeletons, Blight.DarkFog, Blight.Shroud, Blight.Vampire, Blight.UnholyAura, Blight.Taint},
                           new[] {Find.Artifact, Find.Key, Find.SupplyCache, Find.VanishingDust, Find.TreasureChest, Find.Artifact}),
                       new Map(new[] {Blight.Taint, Blight.Curse, Blight.Spies, Blight.Zombies, Blight.DarkFog, Blight.Skeletons, Blight.Desecration},
                           new[] {Find.SupplyCache, Find.TreasureChest, Find.SupplyCache, Find.Waystone, Find.SupplyCache, Find.TreasureChest}),
                       new Map(new[] {Blight.Corruption, Blight.Spies, Blight.Curse, Blight.Vampire, Blight.Zombies, Blight.Shroud, Blight.UnholyAura},
                           new[] {Find.Key, Find.SupplyCache, Find.VanishingDust, Find.BottledMagic, Find.SupplyCache, Find.TreasureChest}),
                       new Map(new[] {Blight.Taint, Blight.Curse, Blight.Vampire, Blight.Shades, Blight.Zombies, Blight.Spies, Blight.Confusion},
                           new[] {Find.TreasureChest, Find.SupplyCache, Find.TreasureChest, Find.Epiphany, Find.Key, Find.Key}),
                       new Map(new[] {Blight.Vampire, Blight.Desecration, Blight.EvilPresence, Blight.Zombies, Blight.Confusion, Blight.Shades, Blight.Shroud},
                           new[] {Find.Waystone, Find.TreasureChest, Find.ForgottenShrine, Find.Epiphany, Find.TreasureChest, Find.Key}),
                       new Map(new[] {Blight.Confusion, Blight.Spies, Blight.Curse, Blight.UnholyAura, Blight.Shroud, Blight.Zombies, Blight.Corruption},
                           new[] {Find.TreasureChest, Find.TreasureChest, Find.Key, Find.Artifact, Find.TreasureChest, Find.Key}),
                       new Map(new[] {Blight.UnholyAura, Blight.Shades, Blight.Zombies, Blight.Shroud, Blight.Desecration, Blight.Skeletons, Blight.Curse},
                           new[] {Find.Key, Find.SupplyCache, Find.Artifact, Find.Artifact, Find.Key, Find.Key}),
                       new Map(new[] {Blight.Corruption, Blight.DarkFog, Blight.Zombies, Blight.Taint, Blight.Spies, Blight.Skeletons, Blight.Curse},
                           new[] {Find.Key, Find.TreasureChest, Find.BottledMagic, Find.TreasureChest, Find.Key, Find.Key}),
                       new Map(new[] {Blight.Shades, Blight.Lich, Blight.Curse, Blight.Desecration, Blight.EvilPresence, Blight.Taint, Blight.Spies},
                           new[] {Find.Key, Find.SupplyCache, Find.Key, Find.Epiphany, Find.Key, Find.Waystone}),
                   };
        }

        private void PopulateInitialBlights(Game game)
        {
            var locations = game.Board.Spaces.Select(space => space.Location).Where(loc => loc != Location.Monastery);
            game.CreateBlight(locations);
        }
    }
}
