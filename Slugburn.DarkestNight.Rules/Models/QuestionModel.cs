namespace Slugburn.DarkestNight.Rules.Models
{
    public class QuestionModel
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public string[] Answers { get; set; }

        public QuestionModel(string title, string text, string[] answers)
        {
            Title = title;
            Text = text;
            Answers = answers;
        }
    }
}
