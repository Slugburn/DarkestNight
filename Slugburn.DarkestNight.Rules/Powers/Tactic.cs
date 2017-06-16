using System;
using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Blights.Implementations;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Powers
{
    public abstract class Tactic : Power
    {
        public Func<int> GetDieCount { get; protected set; }

        public static Tactic None(Hero hero) => new NoTactic(hero);

        public TacticType TacticType { get; protected set; }

        protected Tactic(TacticType tacticType, int dieCount) :this(tacticType)
        {
            GetDieCount = ()=>dieCount;
        }

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

        public virtual void OnSuccess()
        {
        }


        private class NoTactic : Tactic
        {
            public NoTactic(Hero hero) : base(TacticType.Elude | TacticType.Fight, 1)
            {
                Name = "None";
                Hero = hero;
            }
        }

        public int GetResult()
        {
            if (!IsUsable())
                throw new PowerNotUsableException(this);
            var baseDieCount = GetDieCount();
            var adjustedDieCount = AdjustDieCount(baseDieCount);
            var roll = RollDice(adjustedDieCount);
            var result = roll.Max();
            return result;
        }

        private int AdjustDieCount(int baseDieCount)
        {
            var context = new RollContext(baseDieCount);
            var dieBonuses = Hero.GetPowers<IDieBonus>().ToList();
            dieBonuses.ForEach(x=>x.ModifyDice(context));
            return context.DieCount;
        }

        internal virtual IEnumerable<int> RollDice(int count)
        {
            var roll = Hero.Player.RollDice(count);
            return roll;
        }
    }
}
