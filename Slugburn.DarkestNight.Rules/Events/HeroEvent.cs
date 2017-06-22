using System.Collections.Generic;

namespace Slugburn.DarkestNight.Rules.Events
{
    public class HeroEvent
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public List<HeroEventOption> Options { get; set; }
        public bool IsIgnorable { get; set; }
        public string SelectedOption { get; set; }
        public int Fate { get; set; }
        public List<HeroEventRow> Rows { get; set; }

        public void AddOption(string code, string optionText)
        {
            Options.Add(new HeroEventOption {Code= code, Text = optionText});
        }
    }
}