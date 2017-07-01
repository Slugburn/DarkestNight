namespace Slugburn.DarkestNight.Rules.Modifiers
{
    public class ModifierDetail
    {
        public static ModifierDetail Create(string name, int modifier)
        {
            return new ModifierDetail() {Name = name, Modifier = modifier};
        }

        public string Name { get; set; }
        public int Modifier { get; set; }
    }
}