using System.Linq;
using Slugburn.DarkestNight.Rules.Blights;

namespace Slugburn.DarkestNight.Rules.Powers
{
    abstract class Tactic : Power
    {
        protected TacticType TacticType { get; set; }

        protected Tactic(TacticType tacticType)
        {
            Type = PowerType.Tactic;
            TacticType = tacticType;
        }

        public override bool IsUsable()
        {
            // Confusion blight prevents use of tactic powers
            return base.IsUsable() && !Hero.GetBlights().Any(x => x is Confusion);
        }

        public virtual void OnSuccess() { }
    }
}
