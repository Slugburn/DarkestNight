using Slugburn.DarkestNight.Rules.Models;

namespace Slugburn.DarkestNight.Wpf.ViewModels
{
    public class Blight
    {
        public Blight(BlightModel model)
        {
            Name = model.Name;
            Effect = model.Effect;
            Might = model.Might;
            Defense = model.Defense;
        }

        public string Name { get; set; }
        public string Effect { get; set; }
        public int Might { get; set; }
        public string Defense { get; set; }
    }
}
