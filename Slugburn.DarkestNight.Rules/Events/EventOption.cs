namespace Slugburn.DarkestNight.Rules.Events
{
    public class EventOption
    {
        public EventOption(string code, string text)
        {
            Code = code;
            Text = text;
        }

        public EventOption()
        {
        }

        public string Code { get; set; }
        public string Text { get; set; }

        public static EventOption Continue()
        {
            return new EventOption {Code = "cont", Text = "Continue"};
        }
    }
}