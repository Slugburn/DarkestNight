namespace Slugburn.DarkestNight.Rules.Triggers
{
    public abstract class GameTriggerHandler : ITriggerHandler<Game>
    {
        public string Name { get; set; }
        public abstract void HandleTrigger(Game game, TriggerContext context);
    }
}