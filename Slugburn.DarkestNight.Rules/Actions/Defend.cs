using System.Linq;
using Slugburn.DarkestNight.Rules.Enemies;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Players;
using Slugburn.DarkestNight.Rules.Players.Models;
using Slugburn.DarkestNight.Rules.Powers;
using Slugburn.DarkestNight.Rules.Rolls;
using Slugburn.DarkestNight.Rules.Tactics;
using Slugburn.DarkestNight.Rules.Triggers;

namespace Slugburn.DarkestNight.Rules.Actions
{
    public class Defend : IAction, IRollHandler
    {
        public string Name => "Defend";

        public void Act(Hero hero)
        {
            var enemies = hero.Enemies.Select(EnemyFactory.Create);
            hero.ConflictState = new ConflictState
            {
                MaxTarget = 1,
                MinTarget = 1,
                AvailableTargets = enemies.GetTargetInfo(),
                AvailableTactics = hero.GetAvailableTactics().GetInfo(hero)
            };
            hero.SetRollHandler(this);
            hero.State = HeroState.SelectingTarget;
            hero.Player.DisplayConflict(PlayerConflict.FromConflictState(hero.ConflictState));
            hero.Player.State = PlayerState.Conflict;
        }

        public bool IsAvailable(Hero hero)
        {
            return hero.Enemies != null && hero.Enemies.Any();
        }

        public void HandleRoll(Hero hero)
        {
            var target = hero.ConflictState.SelectedTargets.Single();
            var tactic = hero.ConflictState.SelectedTactic;
            var tacticType = tactic.Type;
            if (tacticType == TacticType.Elude)
                hero.Triggers.Send(HeroTrigger.Eluding);
            var targetNumber = tacticType == TacticType.Fight ? target.FightTarget : target.EludeTarget;
            var result = hero.Roll.Max();
            var enemy = EnemyFactory.Create(target.Name);
            if (result < targetNumber)
            {
                enemy.Failure(hero);
            }
            else
            {
                enemy.Win(hero);
                if (tacticType == TacticType.Fight)
                    hero.Triggers.Send(HeroTrigger.FightWon);
            }
        }
    }
}
