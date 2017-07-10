using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Rolls;

namespace Slugburn.DarkestNight.Rules.Events
{
    public class EventRollHandler : IRollHandler
    {
        private readonly EventDetail _detail;

        public EventRollHandler(EventDetail detail)
        {
            _detail = detail;
        }

        public RollState HandleRoll(Hero hero, RollState rollState)
        {
            var e = hero.CurrentEvent;
            e.Rows.Activate(rollState.Result);
            var result = rollState.Result;
            e.Options = _detail.GetHeroEventOptions(hero, result);
            hero.UpdateAvailableCommands();
            hero.DisplayCurrentEvent();
            return rollState;
        }

        public void AcceptRoll(Hero hero, RollState rollState)
        {
        }
    }
}