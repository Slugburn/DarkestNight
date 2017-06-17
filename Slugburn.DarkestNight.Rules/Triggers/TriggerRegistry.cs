using System.Collections.Generic;

namespace Slugburn.DarkestNight.Rules.Triggers
{
    public class TriggerRegistry<T> 
    {
        private readonly ITriggerRegistrar _registrar;
        private readonly Dictionary<T, List<TriggeredAction>> _actions;

        public TriggerRegistry(ITriggerRegistrar registrar)
        {
            _registrar = registrar;
            _actions = new Dictionary<T,List<TriggeredAction>>();
        }

        public void Register(ITriggerHandler handler, T trigger, string tag = null)
        {
            tag = tag ?? trigger.ToString();
            if (!_actions.ContainsKey(trigger))
                _actions[trigger] = new List<TriggeredAction>();
            _actions[trigger].Add(new TriggeredAction(handler.Name, tag));
        }

        public bool Handle(T trigger, object state = null)
        {
            if (!_actions.ContainsKey(trigger)) return true;
            var actions = _actions[trigger];
            bool cancel = false;
            foreach (var action in actions)
            {
                var handler = _registrar.GetTriggerHandler(action.HandlerName);
                var context = new TriggerContext(state);
                handler.HandleTrigger(context, action.Tag);
                cancel = cancel || context.Cancel;
            }
            return !cancel;
        }

        private class TriggeredAction 
        {
            public string HandlerName { get; }
            public string Tag { get; }

            public TriggeredAction(string handlerName, string tag)
            {
                HandlerName = handlerName;
                Tag = tag;
            }
        }
    }
}