namespace Slugburn.DarkestNight.Rules.Models
{
    public class HeroValueModel
    {
        public HeroValueModel(string name, int value, int @default)
        {
            Name = name;
            Value = value;
            Default = @default;
        }

        public string Name { get; set; }
        public int Value { get; set; }
        public int Default { get; set; }
    }
}