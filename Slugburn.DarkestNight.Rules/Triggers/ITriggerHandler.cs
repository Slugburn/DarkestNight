namespace Slugburn.DarkestNight.Rules.Triggers
{
    public interface ITriggerHandler<in T> 
    {
        void HandleTrigger(T registrar, string source, TriggerContext context);
    }
}