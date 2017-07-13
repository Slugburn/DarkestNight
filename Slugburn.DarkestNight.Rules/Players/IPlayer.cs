using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Slugburn.DarkestNight.Rules.Heroes;
using Slugburn.DarkestNight.Rules.Models;

namespace Slugburn.DarkestNight.Rules.Players
{
    public interface IPlayer
    {
        PlayerState State { get; set; }
        void DisplayEvent(EventModel playerEvent);
        void DisplayConflict(ConflictModel conflict);
        void DisplayPowers(ICollection<PowerModel> models, Callback<string> callback);
        Task<IEnumerable<int>> SelectBlights(BlightSelectionModel model);
        Task<Location> SelectLocation(ICollection<string> locations);
        void DisplayNecromancer(NecromancerModel model);
        [Obsolete("Kill it with fire!", true)]
        void DisplayHeroSelection(HeroSelectionModel model, Callback<Hero> callback);
        Task<Hero> SelectHero(HeroSelectionModel model);
        Task<string> AskQuestion(QuestionModel model);
        void DisplaySearch(SearchModel model, Callback<Find> callback);
        void DisplayPrayer(PrayerModel model);
        void AddHero(HeroModel view);
        void UpdateBoard(BoardModel model);
        void UpdateHeroCommands(HeroActionModel model);
        void UpdateHeroStatus(string heroName, HeroStatusModel status);
        void OnNewDay();
    }
}
