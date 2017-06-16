using System;
using System.Collections.Generic;
using System.Linq;

namespace Slugburn.DarkestNight.Rules
{
    public class Stash
    {
        readonly List<object> _items = new List<object>();

        public void Add(params object[] items)
        {
            _items.AddRange(items);
        }

        public T Get<T>()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetAll<T>()
        {
            return _items.Where(x=>x is T).Cast<T>();
        }

        public void Remove(object item)
        {
            _items.Remove(item);
        }

        public void RemoveBySource<T>(string name)
        {
            _items.RemoveAll(x => x is T && x is ISource && ((ISource) x).Name == name);
        }
    }
}
