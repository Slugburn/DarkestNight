using System.Collections.Generic;
using Slugburn.DarkestNight.Rules.Models;

namespace Slugburn.DarkestNight.Rules.Players
{
    public interface IPlayer
    {
        PlayerState State { get; set; }
        void DisplayEvent(EventModel playerEvent);
        void DisplayConflict(ConflictModel conflict);
        void DisplayPowers(ICollection<PowerModel> powers, Callback callback);
        void DisplayBlightSelection(BlightSelectionModel blightSelection, Callback callback);
        void DisplayLocationSelection(ICollection<string> locations, Callback callback);
        void DisplayNecromancer(NecromancerModel model, Callback callback);
        void DisplayHeroSelection(HeroSelectionModel model, Callback callback);
        void DisplayAskQuestion(QuestionModel view, Callback callback);
        void DisplaySearch(SearchModel model, Callback callback);
        void DisplayPrayer(PlayerPrayer view);
        void AddHero(HeroModel view);
        void UpdateBoard(BoardModel view);
        void UpdateHeroCommands(string heroName, IEnumerable<PowerModel> powers, IEnumerable<CommandModel> commands);
        void UpdateHeroStatus(string heroName, HeroStatusModel status);
    }
}
