using System.Linq;
using System.Threading.Tasks;
using Slugburn.DarkestNight.Rules.Commands;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Actions
{
    internal class TravelHandler
    {
        public static async Task UseAvailableMovement(Hero hero)
        {
            DoneMovingCommand doneMovingCommand = null;
            while (hero.AvailableMovement > 0)
            {
                hero.State = HeroState.Moving;
                var validLocations = hero.GetValidMovementLocations().Select(x => x.ToString()).ToList();
                var travelSpeed = hero.TravelSpeed;
                if (travelSpeed > 1 && hero.AvailableMovement < travelSpeed )
                {
                    doneMovingCommand = new DoneMovingCommand(hero);
                    hero.AddCommand(doneMovingCommand);
                    hero.UpdateAvailableCommands();
                }
                var response = await hero.SelectMoveDestination(validLocations);
                if (response.Stop)
                {
                    hero.AvailableMovement = 0;
                }
                else
                {
                    hero.MoveTo(response.Destination);
                    hero.AvailableMovement--;
                }
            }
            if (doneMovingCommand != null)
                hero.RemoveCommand(doneMovingCommand);
            hero.State = HeroState.FinishedMoving;
            hero.ContinueTurn();
        }

        internal class DoneMovingCommand : ICommand
        {
            private readonly Hero _hero;

            public DoneMovingCommand(Hero hero)
            {
                _hero = hero;
            }

            public string Name => "Done Moving";
            public string Text => "Forfeit the remainder of available movement.";
            public bool RequiresAction => false;
            public bool IsAvailable(Hero hero) => true;
            public void Execute(Hero hero)
            {
                _hero.Player.StopMoving();
            }
        }
    }
}