using System;
using System.Collections.Generic;
using System.Linq;
using Slugburn.DarkestNight.Rules.Blights.Implementations;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Triggers;

namespace Slugburn.DarkestNight.Rules.Powers
{
    public abstract class TacticPower : Power
    {
        public Func<int> GetBaseDieCount { get; protected set; }

        public static TacticPower None(Hero hero) => new NoTactic(hero);

        public TacticType TacticType { get; protected set; }

        protected TacticPower(TacticType tacticType, int dieCount) :this(tacticType)
        {
            GetBaseDieCount = ()=>dieCount;
        }

        protected TacticPower(TacticType tacticType)
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


        private class NoTactic : TacticPower
        {
            public NoTactic(Hero hero) : base(TacticType.Elude | TacticType.Fight, 1)
            {
                Name = "None";
                Hero = hero;
            }

            public override bool IsUsable() => true;
        }

        public int GetResult()
        {
            if (!IsUsable())
                throw new PowerNotUsableException(this);
            var baseDieCount = GetBaseDieCount();
            if (baseDieCount == 0) return 0;
            var adjustedDieCount = AdjustDieCount(baseDieCount);
            var roll = GetRoll(adjustedDieCount);
            var result = roll.Max();
            return result;
        }

        public virtual List<int> GetRoll(int dieCount)
        {
            var roll = Hero.Player.RollDice(dieCount).ToList();
            var state = new AfterRollState(roll);
            do
            {
                Hero.Triggers.Handle(HeroTrigger.AfterRoll, state);
            } while (state.Repeat);
            return roll;
        }

        private int AdjustDieCount(int baseDieCount)
        {
            var context = new RollContext(baseDieCount);
            var dieBonuses = Hero.GetPowers<IDieBonus>().ToList();
            dieBonuses.ForEach(x=>x.ModifyDice(context));
            return context.DieCount;
        }
    }
}
