using System;
using System.Collections.Generic;
using Slugburn.DarkestNight.Rules.Heroes;

namespace Slugburn.DarkestNight.Rules.Models
{
    public class PrayerModel
    {
        public List<int> Roll { get; set; }
        public int After { get; set; }
        public int Before { get; set; }
        public int Gain { get; set; }

        public static PrayerModel From(Hero hero)
        {
            var maxGain = Math.Max(0, hero.DefaultGrace - hero.Grace);
            var gain = Math.Min(hero.CurrentRoll.Successes, maxGain);
            var prayer = new PrayerModel
            {
                Roll = hero.CurrentRoll.AdjustedRoll,
                Gain = gain,
                Before = hero.Grace,
                After = hero.Grace + gain
            };
            return prayer;
        }
    }
}