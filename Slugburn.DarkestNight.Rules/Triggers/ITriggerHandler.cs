namespace Slugburn.DarkestNight.Rules.Triggers
{
    public interface ITriggerHandler<in T> : ISource
    {
        void HandleTrigger(T registrar, TriggerContext context);
    }
}