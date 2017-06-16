namespace Slugburn.DarkestNight.Rules.Triggers
{
    public interface ITriggerHandler : ISource
    {
        void HandleTrigger(TriggerContext context, string tag);
    }
}