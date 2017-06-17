namespace Slugburn.DarkestNight.Rules.Triggers
{
    public class TriggerContext
    {
        private readonly object _state;

        public TriggerContext(object state)
        {
            _state = state;
        }

        public bool Cancel { get; set; }

        public T GetState<T>()
        {
            return (T) _state;
        }
    }
}