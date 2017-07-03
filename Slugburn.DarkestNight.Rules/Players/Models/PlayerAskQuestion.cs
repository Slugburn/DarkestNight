using System;

namespace Slugburn.DarkestNight.Rules.Players.Models
{
    public class PlayerAskQuestion
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public string[] Answers { get; set; }

        public PlayerAskQuestion(string title, string text, string[] answers)
        {
            Title = title;
            Text = text;
            Answers = answers;
        }
    }
}
