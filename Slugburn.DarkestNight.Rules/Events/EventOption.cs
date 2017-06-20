namespace Slugburn.DarkestNight.Rules.Events
{
    public class EventOption
    {
        public string Code { get; set; }
        public string Text { get; set; }

        public static EventOption Continue()
        {
            return new EventOption {Code = "cont", Text = "Continue"};
        }
    }
}