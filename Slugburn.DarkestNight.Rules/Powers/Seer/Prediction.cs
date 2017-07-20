namespace Slugburn.DarkestNight.Rules.Powers.Seer
{
    internal class Prediction : ActionPower
    {
        public Prediction()
        {
            Name = "Prediction";
            StartingPower = true;
            Text = "Roll 2 dice and add them to this card. You may use all dice on this card instead of making any roll. When you do, clear this card.";
        }
    }
}