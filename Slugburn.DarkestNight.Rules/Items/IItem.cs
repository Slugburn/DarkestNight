using System.Collections.Generic;
using Slugburn.DarkestNight.Rules.Commands;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Items
{
    public interface IItem : ISource
    {
        string Text { get; }
        Hero Owner { get; set; }
        int Id { get; }
    }
}
