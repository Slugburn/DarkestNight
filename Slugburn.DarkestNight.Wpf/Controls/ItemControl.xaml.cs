using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Slugburn.DarkestNight.Wpf.Controls
{
    /// <summary>
    /// Interaction logic for ItemControl.xaml
    /// </summary>
    public partial class ItemControl : UserControl
    {
        public ItemControl()
        {
            InitializeComponent();

            MouseMove += OnMouseMove;
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.LeftButton != MouseButtonState.Pressed) return;
            DataObject data = new DataObject(this.DataContext);
            DragDrop.DoDragDrop(this, data, DragDropEffects.Move);
        }

        
    }
}
