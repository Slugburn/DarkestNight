using System.Collections.Generic;

namespace Slugburn.DarkestNight.Rules.Events
{
    public class HeroEvent
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public List<EventOption> Options { get; set; }
        public bool IsIgnorable { get; set; }
        public string SelectedOption { get; set; }
        public int Fate { get; set; }
        public List<EventRow> Rows { get; set; }

        public void AddOption(string code, string optionText)
        {
            Options.Add(new EventOption {Code= code, Text = optionText});
        }
    }
}