using Slugburn.DarkestNight.Rules.Commands;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Models;
using Slugburn.DarkestNight.Rules.Players;

namespace Slugburn.DarkestNight.Rules.Items.Artifacts
{
    public class GhostMail : Artifact, IStartOfTurnCommand, ICallbackHandler
    {
        public GhostMail() : base("Ghost Mail")
        {
            Text = @"At the start of your turn, you may spend 1 Grace to gain 1 Secrecy (up to default), "
                   + "or spend 1 Secrecy to gain 1 Grace (up to default).";
        }

        public bool IsAvailable(Hero hero)
        {
            return hero.State == HeroState.TurnStarted
                   && (hero.CanSpendGrace && hero.Secrecy < hero.DefaultSecrecy
                       || hero.CanSpendSecrecy && hero.Grace < hero.DefaultGrace);
        }

        public void Execute(Hero hero)
        {
            var question = new QuestionModel(Name, Text, new[] {"Spend Grace", "Spend Secrecy"});
            hero.Player.DisplayAskQuestion(question, Callback.ForCommand(hero, this));
        }

        public void HandleCallback(Hero hero, string path, object data)
        {
            var answer = (string) data;
            if (answer == "Spend Grace")
            {
                hero.SpendGrace(1);
                hero.GainSecrecy(1, hero.DefaultSecrecy);
            }
            else if (answer == "Spend Secrecy")
            {
                hero.SpendSecrecy(1);
                hero.GainGrace(1, hero.DefaultGrace);
            }
            hero.ContinueTurn();
        }
    }
}
