using System;
using System.Linq;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Extensions;
using Slugburn.DarkestNight.Rules.Tests.Fakes;
using Slugburn.DarkestNight.Rules.Tests.Fluent.Arrangements;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Actions
{
    public class PlayerActionContext : When, IPlayerActionContext
    {
        public PlayerActionContext(Game game, FakePlayer player) : base(game, player)
        {
        }

        public IPlayerActionContext TakesAction(string actionName)
        {
            _player.TakeAction(_player.ActiveHero, actionName);
            return this;
        }


        public IPlayerActionContext UsePower(string name, bool response = true)
        {
            _player.SetUsePowerResponse(name, response);
            return this;
        }

        public IPlayerActionContext ChoosesBlight(params string[] blights)
        {
            _player.SetBlightChoice(blights.Select(x => x.ToEnum<Blight>()).ToArray());
            return this;
        }

        public IPlayerActionContext ChooseLocation(Location location)
        {
            _player.SetLocationChoice(location);
            return this;
        }

        public IPlayerActionContext SelectsEventOption(string option, Action<IFakeRollContext> action = null)
        {
            action?.Invoke(this);
            _player.SelectEventOption(option);
            return this;
        }

        public IPlayerActionContext AcceptsRoll()
        {
            _player.AcceptRoll();
            return this;
        }

        public IPlayerActionContext SelectsLocation(string location)
        {
            _player.SelectLocation(location.ToEnum<Location>());
            return this;
        }

        public IPlayerActionContext ResolvesConflict(Action<ResolveConflictContext> action)
        {
            var context = new ResolveConflictContext(_player.Conflict);
            action(context);
            _player.ResolveConflict(context.GetTactic(), context.GetTargetIds());
            return this;
        }

        public IPlayerActionContext SelectsPower(string powerName)
        {
            _player.SelectPower(powerName);
            return this;
        }

        public IPlayerActionContext SelectsBlight(string location, string blight)
        {
            _player.SelectBlight(location.ToEnum<Location>(), blight.ToEnum<Blight>());
            return this;
        }
    }
}