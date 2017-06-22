namespace Slugburn.DarkestNight.Rules.Events
{
    public class HeroEventOption
    {
        public HeroEventOption(string code, string text)
        {
            Code = code;
            Text = text;
        }

        public HeroEventOption()
        {
        }

        public string Code { get; set; }
        public string Text { get; set; }

        public static HeroEventOption Continue()
        {
            return new HeroEventOption("cont", "Continue");
        }
    }
}