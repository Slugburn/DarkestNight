namespace Slugburn.DarkestNight.Rules
{
    public interface IHero
    {
        int Grace { get; set; }
        int Secrecy { get; set; }
        Location Location { get; set; }

        void StartTurn();
        void EndTurn();
        void LoseTurn();
        void ExhaustPowers();
        void LoseGrace();
        void TakeWound();
        void DrawEvent();
        void LoseSecrecy();
        void ChooseAction();
    }
}