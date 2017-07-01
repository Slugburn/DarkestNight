using Slugburn.DarkestNight.Rules.Commands;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Items.Artifacts
{
    public class GhostMail : Artifact, ICommand
    {
        public const string CommandName = "Start Turn [Ghost Mail]";

        public GhostMail() : base("Ghost Mail")
        {
            Text = @"At the start of your turn, you may spend 1 Grace to gain 1 Secrecy (up to default), "
                   + "or spend 1 Secrecy to gain 1 Grace (up to default).";
        }

        string ICommand.Name => CommandName;

        public bool IsAvailable(Hero hero)
        {
            return StartTurn.IsValid(hero) && (hero.CanSpendGrace || hero.CanSpendSecrecy);
        }

        public void Execute(Hero hero)
        {
            throw new System.NotImplementedException();
        }

    }
}
