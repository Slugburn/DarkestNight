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
        private IRollHandler _primaryRollHandler;

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
        public Hero Hero { get; }

        public bool IsAccepted { get; set; }

        public async void Roll()
        {
            var total = Hero.GetModifiedTotal(BaseDiceCount, ModifierType);
            ActualRoll = Die.Roll(total);
            AdjustRoll();
            await Hero.Triggers.Send(HeroTrigger.Rolled, ModifierType);
        }

        public List<int> Accept()
        {
            var finalRoll = AdjustedRoll;
            IsAccepted = true;
            _rollHandlers.ForEach(x => x.AcceptRoll(Hero, this));
            _primaryRollHandler.AcceptRoll(Hero, this);
            return finalRoll;
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
            var adjusted = Hero.GetRollModifiers()
                .Aggregate<IRollModifier, ICollection<int>>(ActualRoll, (current, modifier) => modifier.Modify(Hero, ModifierType, current));
            AdjustedRoll = adjusted.ToList();
            _rollHandlers.ForEach(x => x.HandleRoll(Hero, this));
            _primaryRollHandler.HandleRoll(Hero, this);
        }

        public void ForceWin()
        {
            AdjustedRoll = new List<int> {6};
        }

        public void SetPrimaryRollHandler(IRollHandler handler)
        {
            _primaryRollHandler = handler;
        }

        public List<int> AddDice(int count)
        {
            var roll = Die.Roll(count);
            ActualRoll.AddRange(roll);
            AdjustRoll();
            return roll;
        }
    }
}