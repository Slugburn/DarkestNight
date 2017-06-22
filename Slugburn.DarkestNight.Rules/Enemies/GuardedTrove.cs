﻿using System;
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
                hero.PresentCurrentEvent();
        }

        public override IEnumerable<ConflictResult> GetResults()
        {
            yield return new ConflictResult("Win", "Lose 1 Secrecy and draw a search result");
            yield return new ConflictResult("Fail fight", "Take wound");
            yield return new ConflictResult("Fail elude", "Spend 1 Secrecy or draw another event");
        }
    }

}