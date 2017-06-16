namespace Slugburn.DarkestNight.Rules.Triggers
{
    public interface ITriggerRegistrar
    {
        ITriggerHandler GetTriggerHandler(string handlerName);
    }
}