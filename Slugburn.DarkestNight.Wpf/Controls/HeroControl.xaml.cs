using Slugburn.DarkestNight.Wpf.ViewModels;
using UserControl = System.Windows.Controls.UserControl;

namespace Slugburn.DarkestNight.Wpf.Controls
{
    /// <summary>
    /// Interaction logic for HeroControl.xaml
    /// </summary>
    public partial class HeroControl : UserControl
    {
        public HeroControl()
        {
            InitializeComponent();
        }

        private void UserControl_Drop(object sender, System.Windows.DragEventArgs e)
        {
            var itemVm = e.Data.GetData(typeof(ItemVm)) as ItemVm;
            if (itemVm == null) return;
            var vm = (HeroVm) DataContext;
            vm.ReceiveItem(itemVm.Owner, itemVm.Id);
        }
    }
}
