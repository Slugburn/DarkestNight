using System.Collections.Generic;
using Slugburn.DarkestNight.Rules.Commands;

namespace Slugburn.DarkestNight.Rules.Heroes
{
    public class ActionFilter : IActionFilter
    {
        private readonly HashSet<string> _allowed;

        public ActionFilter(string name, IEnumerable<string> allowed)
        {
            Name = name;
            _allowed = new HashSet<string>(allowed);
        }

        public string Name { get; }

        public bool IsAllowed(ICommand command)
        {
            return _allowed.Contains(command.Name);
        }
    }
}