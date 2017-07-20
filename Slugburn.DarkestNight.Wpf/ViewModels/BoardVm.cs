using System.Collections.Generic;
using System.Linq;

namespace Slugburn.DarkestNight.Wpf.ViewModels
{
    public class BoardVm
    {
        public LocationVm Monastery { get; set; }
        public LocationVm Mountains { get; set; }
        public LocationVm Castle { get; set; }
        public LocationVm Ruins { get; set; }
        public LocationVm Swamp { get; set; }
        public LocationVm Forest { get; set; }
        public LocationVm Village { get; set; }

        public static BoardVm Create(IEnumerable<LocationVm> locations)
        {
            var byName = locations.ToDictionary(x => x.Name);
            return new BoardVm
            {
                Monastery = byName["Monastery"],
                Mountains = byName["Mountains"],
                Castle = byName["Castle"],
                Ruins = byName["Ruins"],
                Swamp = byName["Swamp"],
                Forest = byName["Forest"],
                Village = byName["Village"]
            };
        }
    }
}