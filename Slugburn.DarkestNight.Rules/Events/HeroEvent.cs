using System.Collections.Generic;

namespace Slugburn.DarkestNight.Rules.Events
{
    public class HeroEvent
    {
        public string Name { get; set; }
        public List<string> Text { get; set; }
        public List<EventOption> Options { get; set; }
        public bool IsIgnorable { get; set; }
        public string SelectedOption { get; set; }

        public void AddOption(string code, string optionText)
        {
            Options.Add(new EventOption {Code= code, Text = optionText});
        }
    }
}