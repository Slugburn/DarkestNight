using System.Collections.Generic;
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
        void DisplayBlightSelection(BlightSelectionModel model);
        void DisplayLocationSelection(ICollection<string> locations, Callback<Location> callback);
        void DisplayNecromancer(NecromancerModel model, Callback<object> callback);
        void DisplayHeroSelection(HeroSelectionModel model, Callback<Hero> callback);
        void DisplayAskQuestion(QuestionModel model, Callback<string> callback);
        void DisplaySearch(SearchModel model, Callback<Find> callback);
        void DisplayPrayer(PrayerModel model);
        void AddHero(HeroModel view);
        void UpdateBoard(BoardModel model);
        void UpdateHeroCommands(HeroActionModel model);
        void UpdateHeroStatus(string heroName, HeroStatusModel status);
    }
}
