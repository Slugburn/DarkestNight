using System;
using System.Collections.Generic;
using System.Linq;

namespace Slugburn.DarkestNight.Rules.Triggers
{
    public class TriggerRegistry<T>
    {
        private readonly Dictionary<T, List<ITriggerHandler>> _handlers;
        private readonly ITriggerRegistrar _registrar;

        public TriggerRegistry(ITriggerRegistrar registrar)
        {
            _registrar = registrar;
            _handlers = new Dictionary<T, List<ITriggerHandler>>();
        }

        public void Register(ITriggerHandler handler, T trigger, string tag = null)
        {
            tag = tag ?? trigger.ToString();
            if (!_handlers.ContainsKey(trigger))
                _handlers[trigger] = new List<ITriggerHandler>();
            _handlers[trigger].Add(handler);
        }

        public bool Handle(T trigger, object state = null)
        {
            if (!_handlers.ContainsKey(trigger)) return true;
            var handlers = _handlers[trigger];
            var cancel = false;
            foreach (var handler in handlers.ToList())
            {
                var context = new TriggerContext(state);
                handler.HandleTrigger(_registrar, context, "");
                cancel = cancel || context.Cancel;
            }
            return !cancel;
        }

        public void Unregister(T trigger, string name)
        {
            var handlers = _handlers[trigger];
            handlers.RemoveAll(x => x.Name == name);
        }
    }
}