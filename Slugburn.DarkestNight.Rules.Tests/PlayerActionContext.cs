using Slugburn.DarkestNight.Rules.Blights;

namespace Slugburn.DarkestNight.Rules.Tests
{
    public class PlayerActionContext
    {
        private readonly FakePlayer _player;
        private readonly FakeDie _die;

        public PlayerActionContext(FakePlayer player, FakeDie die)
        {
            _player = player;
            _die = die;
        }

        public PlayerActionContext UsePower(string name, bool response = true)
        {
            _player.SetUsePowerResponse(name, response);
            return this;
        }

        public PlayerActionContext Rolls(params int[] rolls)
        {
            _die.AddUpcomingRolls(rolls);
            return this;
        }

        public PlayerActionContext ChoosesTactic(string powerName)
        {
            _player.SetTacticChoice(powerName);
            return this;
        }

        public PlayerActionContext ChoosesBlight(params Blight[] blights)
        {
            _player.SetBlightChoice(blights);
            return this;
        }

        public PlayerActionContext AssignRollToBlight(int roll, Blight blight)
        {
            _player.SetBlightRollAssignment(blight, roll);
            return this;
        }

        public PlayerActionContext ChooseLocation(Location location)
        {
            _player.SetLocationChoice(location);
            return this;
        }

        public PlayerActionContext RollAnotherDie(params bool[] choices)
        {
            _player.SetRollAnotherDieChoice(choices);
            return this;
        }
    }
}