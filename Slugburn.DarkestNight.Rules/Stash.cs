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

        public IEnumerable<T> GetAll<T>()
        {
            return _items.Where(x=>x is T).Cast<T>();
        }
    }
}
