using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.IO;

namespace Slugburn.DarkestNight.Rules.Triggers
{
    public class TriggerRegistry<TTrigger, TRegistrar> where TTrigger : struct
    {
        private readonly TRegistrar _registrar;
        private readonly Dictionary<TTrigger, List<Item>> _byTrigger;
        private readonly Dictionary<string, List<Item>> _bySource;

        public TriggerRegistry(TRegistrar registrar)
        {
            _registrar = registrar;
            _byTrigger = new Dictionary<TTrigger, List<Item>>();
            _bySource = new Dictionary<string, List<Item>>();
        }

        public void Add(TTrigger trigger, string source, ITriggerHandler<TRegistrar> handler)
        {
            if (!_byTrigger.ContainsKey(trigger))
                _byTrigger[trigger] = new List<Item>();
            if (!_bySource.ContainsKey(source))
                _bySource[source] = new List<Item>();
            var item = new Item {Trigger = trigger, Source = source, Handler = handler};
            _byTrigger[trigger].Add(item);
            _bySource[source].Add(item);
        }

        public bool Send(TTrigger trigger, object state = null)
        {
            if (!_byTrigger.ContainsKey(trigger)) return true;
            var items = _byTrigger[trigger];
            var cancel = false;
            foreach (var item in items.ToList())
            {
                var context = new TriggerContext(state, trigger);
                item.Handler.HandleTrigger(_registrar, item.Source, context);
                cancel = cancel || context.Cancel;
            }
            return !cancel;
        }

        public void Remove(TTrigger trigger, string source)
        {
            var byTrigger = _byTrigger[trigger];
            byTrigger.RemoveAll(x => x.Source== source);
            var bySource = _bySource[source];
            bySource.RemoveAll(x => x.Trigger.ToString() == trigger.ToString());
        }

        public void RemoveBySource(string source)
        {
            if (!_bySource.ContainsKey(source)) return;
            var bySource = _bySource[source];
            _bySource.Remove(source);
            foreach (var item in bySource)
                _byTrigger[item.Trigger].Remove(item);
        }

        private class Item
        {
            public TTrigger Trigger { get; set; }
            public string Source { get; set; }
            public ITriggerHandler<TRegistrar> Handler { get; set; }
        }
    }
}