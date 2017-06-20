using System;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Rolls;
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
    }

    public interface IFakeRollContext
    {
    }

    public static class FakeRollExtension
    {
        public static T Rolls<T>(this T context, params int[] upcomingRolls) where T : IFakeRollContext
        {
            var die = (FakeDie) Die.Implementation;
            die.AddUpcomingRolls(upcomingRolls);
            return context;
        } 
    }
}