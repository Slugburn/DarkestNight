using System.Threading.Tasks;

namespace Slugburn.DarkestNight.Rules.Triggers
{
    public interface ITriggerHandler<in T> 
    {
        Task HandleTriggerAsync(T registrar, string source, TriggerContext context);
    }
}