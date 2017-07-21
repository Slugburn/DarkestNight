using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using Slugburn.DarkestNight.Rules.Models;

namespace Slugburn.DarkestNight.Wpf.ViewModels {
    public class BarSegment
    {
        public SolidColorBrush Brush { get; set; }
        public decimal Opacity { get; set; }

        public static List<BarSegment> CreateHeroValueBar(Color color, int value, int def)
        {
            var segmentCount = Math.Max(def, value);
            return Enumerable.Range(1, segmentCount)
                .Select(i => new BarSegment
                {
                    Brush = new SolidColorBrush(color),
                    Opacity = i > value ? 0.0m : i > def ? 0.5m : 1.0m
                }).ToList();
        }

        public static List<BarSegment> CreateHeroValueBar(HeroValueModel model, Color color)
        {
            return BarSegment.CreateHeroValueBar(color, model.Value, model.Default);
        }
    }
}