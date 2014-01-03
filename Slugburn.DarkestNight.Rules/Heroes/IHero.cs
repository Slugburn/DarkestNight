using System;
using System.Collections.Generic;
using Slugburn.DarkestNight.Rules.Blights;
using Slugburn.DarkestNight.Rules.Powers;

namespace Slugburn.DarkestNight.Rules.Heroes
{
    public interface IHero
    {
        int DefaultGrace { get; set; }
        int DefaultSecrecy { get; set; }
        int Grace { get; set; }
        int Secrecy { get; set; }
        Location Location { get; set; }
        HeroState State { get; set; }

        ICollection<IPower> Powers { get; }
        string Name { get; set; }

        IEnumerable<IBlight> GetBlights();
        ISpace GetSpace();

        void StartTurn();
        void EndTurn();
        void LoseTurn();
        void ExhaustPowers();
        void LoseGrace();
        void TakeWound();
        void DrawEvent();
        void LoseSecrecy();
        void ChooseAction();
        void Add<T>(T item);
        void Remove<T>(T item);
        ITargetSelectionSyntax<T> SelectTarget<T>(TargetType targetType);
        void MoveTo(Location location);
        void GainSecrecy(int amount, int max);
        void SetDice(RollType rollType, int count);
        void GainGrace(int amount, int max = int.MaxValue);
        IOfferRerollSyntax OfferReroll();
        IChooseDiceSyntax ChooseDice(RollType rollType, int min, int max);
        IEnumerable<int> GetLastRoll(RollType rollType);
    }

    public interface IChooseDiceSyntax
    {
        void AfterFight(System.Action action);
    }

    public interface IOfferRerollSyntax
    {
        void OnReroll(Action<IHero> action);
    }

    public interface ITargetSelectionSyntax<out T>
    {
        void OnTarget(Action<T> action);
    }
}