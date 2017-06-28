using System;

namespace Slugburn.DarkestNight.Rules.Players.Models
{
    public class PlayerAskQuestion
    {
        public string Title { get; set; }
        public string Text { get; set; }

        public PlayerAskQuestion(string title, string text)
        {
            Title = title;
            Text = text;
        }
    }
}
