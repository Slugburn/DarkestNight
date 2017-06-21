using System;
using System.Linq;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Tests.Fakes;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent
{
    public class PlayerActionContext : IFakeRollContext
    {
        private readonly FakePlayer _player;

        public PlayerActionContext(FakePlayer player)
        {
            _player = player;
        }

        public PlayerActionContext TakesAction(string actionName)
        {
            _player.TakeAction(_player.ActiveHero, actionName);
            return this;
        }


        public PlayerActionContext UsePower(string name, bool response = true)
        {
            _player.SetUsePowerResponse(name, response);
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

        public PlayerActionContext SelectsEventOption(string option, Action<IFakeRollContext> action = null)
        {
            action?.Invoke(this);
            _player.SelectEventOption(option);
            return this;
        }

        public void AcceptsRoll()
        {
            _player.AcceptRoll();
        }

        public PlayerActionContext SelectsLocation(Location location)
        {
            _player.SelectLocation(location);
            return this;
        }

        public PlayerActionContext SelectsTactic(string tactic, params string[] targets)
        {
            var targetIds = _player.Conflict.Targets.Where(x => targets.Contains(x.Name)).Select(x => x.Id).ToList();
            _player.SelectTactic(tactic, targetIds);
            return this;
        }
    }
}