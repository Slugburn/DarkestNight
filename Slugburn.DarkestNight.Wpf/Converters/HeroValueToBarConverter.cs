using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;
using Slugburn.DarkestNight.Wpf.ViewModels;

namespace Slugburn.DarkestNight.Wpf.Converters
{
    public class HeroValueToBarConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var hv = (HeroValueVm) value;
            return BarSegment.CreateHeroValueBar(hv.Text == "Grace" ? Colors.White : Colors.Black, hv.Value, hv.Default);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}