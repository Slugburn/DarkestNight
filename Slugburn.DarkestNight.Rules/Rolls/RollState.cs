using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Modifiers;
using Slugburn.DarkestNight.Rules.Triggers;

namespace Slugburn.DarkestNight.Rules.Rolls
{
    public class RollState
    {
        private readonly List<IRollHandler> _rollHandlers = new List<IRollHandler>();

        internal RollState(Hero hero)
        {
            Hero = hero;
        }

        public string BaseName { get; set; }

        public List<int> ActualRoll { get; set; }
        public List<int> AdjustedRoll { get; set; }
        public int TargetNumber { get; set; }
        public ModifierType ModifierType { get; set; }

        public int Result => AdjustedRoll.Max();
        public bool Win => Result >= TargetNumber;
        public int Successes => AdjustedRoll.Count(x => x >= TargetNumber);

        public int BaseDiceCount { get; set; }
        public Hero Hero { get; private set; }

        public void Roll()
        {
            var total = Hero.GetModifiedTotal(BaseDiceCount, ModifierType);
            ActualRoll = Die.Roll(total);
            AdjustedRoll = ActualRoll.ToList();
            _rollHandlers.ForEach(x => x.HandleRoll(Hero, this));
            Hero.Triggers.Send(HeroTrigger.Rolled, ModifierType);
        }

        public void Accept()
        {
            _rollHandlers.ForEach(x => x.AcceptRoll(Hero, this));
        }

        public void AddRollHandler<THandler>() where THandler : IRollHandler, new()
        {
            AddRollHandler(new THandler());
        }

        public void AddRollHandler(IRollHandler handler)
        {
            _rollHandlers.Add(handler);
        }

        public void AdjustRoll()
        {
            // reset the adjusted roll back to the actual roll
            AdjustedRoll = ActualRoll.ToList();
            foreach (var handler in _rollHandlers)
                handler.HandleRoll(Hero, this);
        }
    }
}