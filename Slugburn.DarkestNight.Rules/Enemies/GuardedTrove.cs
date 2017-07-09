using System;
using System.Collections.Generic;
using Slugburn.DarkestNight.Rules.Conflicts;
using Slugburn.DarkestNight.Rules.Events;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Powers;
using Slugburn.DarkestNight.Rules.Tactics;

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
            // finish the conflict before going back to the event
            hero.Enemies.Remove(this);
            hero.ConflictState = null;
            hero.DrawSearchResults(1);
            hero.EndEvent();
        }

        public override void Failure(Hero hero)
        {
            var tacticType = hero.ConflictState.SelectedTactic.Type;
            hero.Enemies.Remove(this);
            hero.ConflictState = null;
            if (tacticType == TacticType.Fight)
            {
                hero.TakeWound();
                hero.EndEvent();
            }
            else
            {
                var options = new List<HeroEventOption>();
                if (hero.CanSpendSecrecy)
                    options.Add(new HeroEventOption("spend-secrecy","Spend Secrecy"));
                options.Add(new HeroEventOption("draw-event", "Draw Event"));
                hero.CurrentEvent.Options = options;
                hero.DisplayCurrentEvent();
            }
        }

        public override string OutcomeDescription(bool isWin, TacticType tacticType)
        {
            if (isWin) return "Lose 1 Secrecy and draw a search result";
            return tacticType == TacticType.Fight 
                ? "Wound" 
                : "Spend 1 Secrecy or draw another event";
        }
    }

}