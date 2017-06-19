using System;
using System.Collections.Generic;
using Slugburn.DarkestNight.Rules.Events;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Powers;

namespace Slugburn.DarkestNight.Rules.Enemies
{
    public class GuardedTrove : Enemy
    {
        public GuardedTrove()
        {
            Name = "Guarded Trove";
            Fight = 6;
            Elude = 6;
        }

        public override void Win(Hero hero)
        {
            hero.LoseSecrecy("Enemy");
            hero.DrawSearchResult();
        }

        public override void Failure(Hero hero)
        {
            var tacticType = hero.ConflictState.SelectedTactic.Type;
            if (tacticType == TacticType.Fight)
                hero.TakeWound();
            else
                hero.PresentEvent(new GuardedTroveEludeFailureEvent());
        }

        public override IEnumerable<ConflictResult> GetResults()
        {
            yield return new ConflictResult("Win", "Lose 1 Secrecy and draw a search result");
            yield return new ConflictResult("Fail fight", "Take wound");
            yield return new ConflictResult("Fail elude", "Spend 1 Secrecy or draw another event");
        }

        public class GuardedTroveEludeFailureEvent : IEventCard
        {
            public string Name => "Guarded Trove (Fail elude)";

            public EventDetail Detail => EventDetail.Create(x => x
                .Text("Spend 1 Secrecy or draw another event")
                .Option("secrecy", "Spend Secrecy", hero => hero.Secrecy > 0)
                .Option("event", "Draw Event"));

            public void Resolve(Hero hero, string option)
            {
                if (option == "secrecy")
                    hero.SpendSecrecy(1);
                else if (option == "event")
                    hero.DrawEvent();
                else
                    throw new ArgumentOutOfRangeException(nameof(option));
            }
        }
    }

}