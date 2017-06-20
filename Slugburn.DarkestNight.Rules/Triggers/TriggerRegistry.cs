using System.Collections.Generic;
using System.Linq;

namespace Slugburn.DarkestNight.Rules.Triggers
{
    public class TriggerRegistry<TTrigger, TRegistrar>
    {
        private readonly Dictionary<TTrigger, List<ITriggerHandler<TRegistrar>>> _handlers;
        private readonly TRegistrar _registrar;

        public TriggerRegistry(TRegistrar registrar)
        {
            _registrar = registrar;
            _handlers = new Dictionary<TTrigger, List<ITriggerHandler<TRegistrar>>>();
        }

        public void Register(TTrigger trigger, ITriggerHandler<TRegistrar> handler)
        {
            if (!_handlers.ContainsKey(trigger))
                _handlers[trigger] = new List<ITriggerHandler<TRegistrar>>();
            _handlers[trigger].Add(handler);
        }

        public bool Handle(TTrigger trigger, object state = null)
        {
            if (!_handlers.ContainsKey(trigger)) return true;
            var handlers = _handlers[trigger];
            var cancel = false;
            foreach (var handler in handlers.ToList())
            {
                var context = new TriggerContext(state);
                handler.HandleTrigger(_registrar, context);
                cancel = cancel || context.Cancel;
            }
            return !cancel;
        }

        public void Unregister(TTrigger trigger, string name)
        {
            var handlers = _handlers[trigger];
            handlers.RemoveAll(x => x.Name == name);
        }

        public void UnregisterAll(string name)
        {
            foreach (var key in _handlers.Keys)
                Unregister(key, name);
        }
    }
}