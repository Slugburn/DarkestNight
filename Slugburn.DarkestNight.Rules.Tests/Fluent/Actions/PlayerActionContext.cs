using System;
using System.Linq;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Extensions;
using Slugburn.DarkestNight.Rules.Tests.Fakes;
using Slugburn.DarkestNight.Rules.Tests.Fluent.Arrangements;

namespace Slugburn.DarkestNight.Rules.Tests.Fluent.Actions
{
    public class PlayerActionContext : WhenContext, IPlayerActionContext
    {
        public PlayerActionContext(Game game, FakePlayer player) : base(game, player)
        {
        }

        public IPlayerActionContext TakesAction(string actionName)
        {
            ((TestRoot) this).GetPlayer().TakeAction(((TestRoot) this).GetPlayer().ActiveHero, actionName);
            return this;
        }


        public IPlayerActionContext UsePower(string name, bool response = true)
        {
            ((TestRoot) this).GetPlayer().SetUsePowerResponse(name, response);
            return this;
        }

        public IPlayerActionContext ChoosesBlight(params string[] blights)
        {
            ((TestRoot) this).GetPlayer().SetBlightChoice(blights.Select(x => x.ToEnum<Blight>()).ToArray());
            return this;
        }

        public IPlayerActionContext ChooseLocation(Location location)
        {
            ((TestRoot) this).GetPlayer().SetLocationChoice(location);
            return this;
        }

        public IPlayerActionContext SelectsEventOption(string option, IFakeRollContext set = null)
        {
            ((TestRoot) this).GetPlayer().SelectEventOption(option);
            return this;
        }

        public IPlayerActionContext AcceptsRoll()
        {
            ((TestRoot) this).GetPlayer().AcceptRoll();
            return this;
        }

        public IPlayerActionContext SelectsLocation(string location)
        {
            ((TestRoot) this).GetPlayer().SelectLocation(location.ToEnum<Location>());
            return this;
        }

        public IPlayerActionContext ResolvesConflict(Action<ResolveConflictContext> action)
        {
            var context = new ResolveConflictContext(((TestRoot) this).GetPlayer().Conflict);
            action(context);
            ((TestRoot) this).GetPlayer().ResolveConflict(context.GetTactic(), context.GetTargetIds());
            return this;
        }

        public IPlayerActionContext SelectsPower(string powerName)
        {
            ((TestRoot) this).GetPlayer().SelectPower(powerName);
            return this;
        }

        public IPlayerActionContext SelectsBlight(string location, string blight)
        {
            ((TestRoot) this).GetPlayer().SelectBlight(location.ToEnum<Location>(), blight.ToEnum<Blight>());
            return this;
        }
    }
}