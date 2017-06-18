namespace Slugburn.DarkestNight.Rules.Triggers
{
    public interface ITriggerHandler : ISource
    {
        void HandleTrigger(ITriggerRegistrar registrar, TriggerContext context, string tag);
    }
}