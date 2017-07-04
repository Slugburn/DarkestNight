namespace Slugburn.DarkestNight.Rules.Models
{
    public class EventOptionModel
    {
        public EventOptionModel(string code, string text)
        {
            Code = code;
            Text = text;
        }

        public string Code { get; set; }
        public string Text { get; set; }
    }
}