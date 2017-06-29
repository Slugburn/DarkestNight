using System;
using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Extensions;
using Slugburn.DarkestNight.Rules.Items;
using Slugburn.DarkestNight.Rules.Players;
using Slugburn.DarkestNight.Rules.Players.Models;
using Slugburn.DarkestNight.Rules.Powers;

namespace Slugburn.DarkestNight.Rules.Heroes
{
    class DrawSearchCommand 
    {
        public void DrawSearchResult(Hero hero)
        {
            var result = hero.Game.DrawSearchResult(hero.Location);
            switch (result)
            {
                case Find.Key:
                    hero.AddToInventory(new Key());
                    break;
                case Find.BottledMagic:
                    hero.AddToInventory(new BottledMagic());
                    break;
                case Find.SupplyCache:
                    var powerNames = hero.PowerDeck.Draw(2);
                    var powers = powerNames.Select(PowerFactory.Create).ToList();
                    var viewModel = PlayerPower.FromPowers(powers).ToList();
                    hero.Player.DisplayPowers(viewModel, Callback.For(hero, new SupplyCacheCallback(powerNames)));
                    break;
                case Find.TreasureChest:
                    hero.AddToInventory(new TreasureChest());
                    break;
                case Find.Waystone:
                    hero.AddToInventory(new Waystone());
                    break;
                case Find.ForgottenShrine:
                    hero.GainGrace(2, int.MaxValue);
                    break;
                case Find.VanishingDust:
                    hero.AddToInventory(new VanishingDust());
                    break;
                case Find.Epiphany:
                    throw new NotImplementedException();
                    break;
                case Find.Artifact:
                    throw new NotImplementedException();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
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
                var selectedName = (string) data;
                var notSelectedName = _powerNames.Single(x => x!=selectedName);
                hero.LearnPower(PowerFactory.Create(selectedName));
                hero.PowerDeck.Add(notSelectedName);
            }
        }
    }
}
