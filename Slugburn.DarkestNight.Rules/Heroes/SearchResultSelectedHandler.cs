using System;
using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Models;
using Slugburn.DarkestNight.Rules.Players;
using Slugburn.DarkestNight.Rules.Powers;

namespace Slugburn.DarkestNight.Rules.Heroes
{
    internal class SearchResultSelectedHandler : ICallbackHandler
    {
        public void HandleCallback(Hero hero, string path, object data)
        {
            var result = (Find) data;
            var game = hero.Game;
            switch (result)
            {
                case Find.Key:
                    hero.AddToInventory(game.CreateItem("Key"));
                    break;
                case Find.BottledMagic:
                    hero.AddToInventory(game.CreateItem("Bottled Magic"));
                    break;
                case Find.SupplyCache:
                    SuppyCache(hero);
                    break;
                case Find.TreasureChest:
                    hero.AddToInventory(game.CreateItem("Treasure Chest"));
                    break;
                case Find.Waystone:
                    hero.AddToInventory(game.CreateItem("Waystone"));
                    break;
                case Find.ForgottenShrine:
                    hero.GainGrace(2, int.MaxValue);
                    break;
                case Find.VanishingDust:
                    hero.AddToInventory(game.CreateItem("Vanishing Dust"));
                    break;
                case Find.Epiphany:
                    Epiphany(hero);
                    break;
                case Find.Artifact:
                    var artifactName = game.ArtifactDeck.Draw();
                    var artifact = game.CreateItem(artifactName);
                    hero.AddToInventory(artifact);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static void SuppyCache(Hero hero)
        {
            var powerNames = hero.PowerDeck.Draw(2);
            var powers = powerNames.Select(PowerFactory.Create).ToList();
            var viewModel = PowerModel.FromPowers(powers).ToList();
            hero.Player.DisplayPowers(viewModel, Callback.For(hero, new SupplyCacheCallback(powerNames)));
        }

        private static void Epiphany(Hero hero)
        {
            var powerNames = hero.PowerDeck;
            var powers = powerNames.Select(PowerFactory.Create).ToList();
            var viewModel = PowerModel.FromPowers(powers).ToList();
            hero.Player.DisplayPowers(viewModel, Callback.For(hero, new EpiphanyCallback()));
        }

        internal class SupplyCacheCallback : ICallbackHandler
        {
            private readonly List<string> _powerNames;

            public SupplyCacheCallback(List<string> powerNames)
            {
                _powerNames = powerNames;
            }

            public void HandleCallback(Hero hero, string path, object data)
            {
                var selectedName = (string)data;
                var notSelectedName = _powerNames.Single(x => x != selectedName);
                hero.LearnPower(PowerFactory.Create(selectedName));
                hero.PowerDeck.Add(notSelectedName);
            }
        }

        private class EpiphanyCallback : ICallbackHandler
        {
            public void HandleCallback(Hero hero, string path, object data)
            {
                var selectedName = (string)data;
                hero.PowerDeck.Remove(selectedName);
                hero.ShufflePowerDeck();
                hero.LearnPower(PowerFactory.Create(selectedName));
            }
        }

    }
}