namespace Slugburn.DarkestNight.Rules.Items.Artifacts
{
    class GhostMail : Artifact
    {
        public GhostMail() : base("Ghost Mail")
        {
            Text = @"At the start of your turn, you may spend 1 Grace to gain 1 Secrecy (up to default), "
                   + "or spend 1 Secrecy to gain 1 Grace (up to default).";
        }
    }
}
