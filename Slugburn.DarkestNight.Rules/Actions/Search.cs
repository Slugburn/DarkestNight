using System;
using Slugburn.DarkestNight.Rules.Extensions;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Rolls;
using Slugburn.DarkestNight.Rules.Triggers;

namespace Slugburn.DarkestNight.Rules.Actions
{
    public class Search : IAction
    {
        public string Name => "Search";
        public void Act(Hero hero)
        {
            hero.Triggers.Send(HeroTrigger.Searching);
            hero.SetRollHandler(new SearchRollHandler());
            var dice = hero.GetSearchDice();
            var rollState = hero.RollDice(dice);
            var space = hero.GetSpace();
            rollState.TargetNumber = space.SearchTarget;
            hero.IsActionAvailable = false;
        }

        public class SearchRollHandler : IRollHandler
        {
            public RollState HandleRoll(Hero hero, RollState rollState)
            {
                return rollState;
            }

            public void AcceptRoll(Hero hero, RollState rollState)
            {
                if (rollState.Win)
                {
                    var map = hero.Game.Maps.Draw();
                    var space = hero.GetSpace();
                    var result = map.GetSearchResult(space.Location);
                    switch (result)
                    {
                            case Find.Key:
                            hero.Inventory.Add("Key");
                            break;
                        case Find.BottledMagic:
                            hero.Inventory.Add("Bottled Magic");
                            break;
                        case Find.SupplyCache:
                            break;
                        case Find.TreasureChest:
                            hero.Inventory.Add("Treasure Chest");
                            break;
                        case Find.Waystone:
                            hero.Inventory.Add("Waystone");
                            break;
                        case Find.ForgottenShrine:
                            break;
                        case Find.VanishingDust:
                            hero.Inventory.Add("Vanishing Dust");
                            break;
                        case Find.Epiphany:
                            break;
                        case Find.Artifact:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                else
                {
                }
            }
        }

        public bool IsAvailable(Hero hero)
        {
            return hero.IsActionAvailable && hero.Location != Location.Monastery;
        }

        public void HandleRoll(Hero hero)
        {
            throw new NotImplementedException();
        }
    }
}
