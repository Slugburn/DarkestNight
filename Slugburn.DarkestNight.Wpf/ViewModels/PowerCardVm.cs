using Slugburn.DarkestNight.Rules.Models;

namespace Slugburn.DarkestNight.Wpf.ViewModels
{
    public class PowerCardVm
    {
        public static PowerCardVm Create(PowerModel model)
        {
            return new PowerCardVm {Name= model.Name, Text = model.Text};
        }

        public string Name { get; set; }

        public string Text { get; set; }
    }
}
