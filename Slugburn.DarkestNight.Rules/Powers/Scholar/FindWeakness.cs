namespace Slugburn.DarkestNight.Rules.Powers.Scholar
{
    internal class FindWeakness : TacticPower
    {
        public FindWeakness()
        {
            Name = "Find Weakness";
            StartingPower = true;
            Text = "Fight with 1 die. Before rolling, pick 1 die, and add 1 to its result.";
        }
    }
}