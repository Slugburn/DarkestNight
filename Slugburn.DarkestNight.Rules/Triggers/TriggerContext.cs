namespace Slugburn.DarkestNight.Rules.Triggers
{
    public class TriggerContext
    {
        private readonly object _state;
        private readonly object _triggeredBy;

        public TriggerContext(object state, object triggeredBy)
        {
            _state = state;
            _triggeredBy = triggeredBy;
        }

        public bool Cancel { get; set; }

        public T GetState<T>()
        {
            return (T) _state;
        }

        public bool WasTriggeredBy<T>(T trigger) where T:struct
        {
            return _triggeredBy.Equals(trigger);
        }
    }
}