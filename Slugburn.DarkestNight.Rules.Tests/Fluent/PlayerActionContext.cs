﻿using System;
using System.Linq;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Extensions;
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

        public PlayerActionContext ChoosesBlight(params string[] blights)
        {
            _player.SetBlightChoice(blights.Select(x=>x.ToEnum<Blight>()).ToArray());
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

        public PlayerActionContext AcceptsRoll()
        {
            _player.AcceptRoll();
            return this;
        }

        public PlayerActionContext SelectsLocation(string location)
        {
            _player.SelectLocation(location.ToEnum<Location>());
            return this;
        }

        public PlayerActionContext ResolvesConflict(Action<ResolveConflictContext> action)
        {
            var context = new ResolveConflictContext(_player.Conflict);
            action(context);
            _player.ResolveConflict(context.GetTactic(), context.GetTargetIds());
            return this;
        }

        public PlayerActionContext SelectsPower(string powerName)
        {
            _player.SelectPower(powerName);
            return this;
        }

        public PlayerActionContext SelectsBlight(string location, string blight)
        {
            _player.SelectBlight(location.ToEnum<Location>(), blight.ToEnum<Blight>());
            return this;
        }
    }
}