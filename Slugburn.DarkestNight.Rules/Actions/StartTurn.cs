﻿using System.Linq;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Triggers;

namespace Slugburn.DarkestNight.Rules.Actions
{
    public class StartTurn : IAction
    {
        public string Name => "Start Turn";
        public void Act(Hero hero)
        {
            hero.IsActing = true;
            hero.Triggers.Send(HeroTrigger.StartTurn);
            if (hero.Location == hero.Game.Necromancer.Location)
                hero.LoseSecrecy("Necromancer");
            hero.State = HeroState.ChoosingAction;
            hero.AvailableActions = hero.GetAvailableActions();
        }

        public bool IsAvailable(Hero hero)
        {
            var game = hero.Game;
            return !hero.IsTurnTaken
                   && !game.Necromancer.IsActing
                   && !game.Heroes.Any(h => h.IsActing);
        }
    }
}