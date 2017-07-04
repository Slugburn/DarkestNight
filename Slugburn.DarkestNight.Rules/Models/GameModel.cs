using System.Collections.Generic;

namespace Slugburn.DarkestNight.Rules.Models
{
    public class GameModel
    {
        public BoardModel Board { get; set; }
        public List<HeroModel> Heroes { get; set; } = new List<HeroModel>();
    }
}
