using System;
using System.Collections.Generic;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Powers;

namespace Slugburn.DarkestNight.Rules.Heroes
{
    public abstract class Hero : IHero
    {
        public int DefaultGrace { get; set; }
        public int DefaultSecrecy { get; set; }
        public int Grace { get; set; }
        public int Secrecy { get; set; }
        public Location Location { get; set; }
        public HeroState State { get; set; }
        public ICollection<IPower> Powers { get; protected set; }
        public string Name { get; set; }

        public IEnumerable<IBlight> GetBlights()
        {
            throw new NotImplementedException();
        }

        public ISpace GetSpace()
        {
            throw new NotImplementedException();
        }

        public void StartTurn()
        {
            throw new System.NotImplementedException();
        }

        public void EndTurn()
        {
            throw new System.NotImplementedException();
        }

        public void LoseTurn()
        {
            throw new System.NotImplementedException();
        }

        public void ExhaustPowers()
        {
            throw new System.NotImplementedException();
        }

        public void LoseGrace()
        {
            throw new System.NotImplementedException();
        }

        public void TakeWound()
        {
            throw new System.NotImplementedException();
        }

        public void DrawEvent()
        {
            throw new System.NotImplementedException();
        }

        public void LoseSecrecy()
        {
            throw new System.NotImplementedException();
        }

        public void ChooseAction()
        {
            throw new System.NotImplementedException();
        }

        public void Add<T>(T item)
        {
            throw new NotImplementedException();
        }

        public void Remove<T>(T item)
        {
            throw new NotImplementedException();
        }

        public ITargetSelectionSyntax<T> SelectTarget<T>(TargetType targetType)
        {
            throw new NotImplementedException();
        }

        public void MoveTo(Location location)
        {
            throw new NotImplementedException();
        }

        public void GainSecrecy(int amount, int max)
        {
            throw new NotImplementedException();
        }

        public void SetDice(RollType rollType, int count)
        {
            throw new NotImplementedException();
        }

        public void GainGrace(int amount, int max)
        {
            throw new NotImplementedException();
        }

        public IOfferRerollSyntax OfferReroll()
        {
            throw new NotImplementedException();
        }

        public IChooseDiceSyntax ChooseDice(RollType rollType, int min, int max)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<int> GetLastRoll(RollType rollType)
        {
            throw new NotImplementedException();
        }
    }

}
