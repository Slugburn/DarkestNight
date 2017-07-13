using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using Slugburn.DarkestNight.Rules.Models;
using Slugburn.DarkestNight.Wpf.ViewModels;

namespace Slugburn.DarkestNight.Wpf.Commands
{
    class SelectBlightsCommand
    {
        private readonly PlayerVm _playerVm;
        private readonly List<BlightVm> _blights;
        private readonly int _max;
        private readonly TaskCompletionSource<IEnumerable<int>> _completionSource;

        public SelectBlightsCommand(PlayerVm playerVm, BlightSelectionModel model)
        {
            _playerVm = playerVm;
            _max = model.Max;
            _blights = GetBlights(model);
            _completionSource = new TaskCompletionSource<IEnumerable<int>>();
        }

        public Task<IEnumerable<int>> Execute()
        {
            if (_max == 1 || _blights.Count == 1)
                SelectSingleBlight();
            else
                SelectMultipleBlights();
            return _completionSource.Task;
        }

        private void SelectSingleBlight()
        {
            foreach (var blight in _blights)
            {
                blight.Highlight = new SolidColorBrush(Colors.Orange);
                blight.Command = new CommandHandler(() => OnSingleBlightSelected(blight));
            }
        }

        private void OnSingleBlightSelected(BlightVm vm)
        {
            _blights.ForEach(x => x.Clear());
            _completionSource.SetResult(new[] { vm.Id });
        }

        private void SelectMultipleBlights()
        {
            var command = new CommandHandler(OnAllBlightsSelected, IsBlightSelectionValid);
            foreach (var blight in _blights)
            {
                blight.IsSelectable = true;
                blight.PropertyChanged += (sender, e) => command.OnCanExecuteChanged();
            }
            _playerVm.Command = command;
        }

        private bool IsBlightSelectionValid()
        {
            var selectedCount = _blights.Count(x => x.IsSelected);
            return selectedCount > 0 && selectedCount <= _max;
        }

        private void OnAllBlightsSelected()
        {
            var selectedIds = _blights.Where(x => x.IsSelected).Select(x => x.Id).ToList();
            _blights.ForEach(x => x.Clear());
            _playerVm.Command = null;
            _completionSource.SetResult(selectedIds);
        }

        private List<BlightVm> GetBlights(BlightSelectionModel model)
        {
            var validIds = model.Blights.Select(x => x.Id);
            var allVms = _playerVm.Locations.SelectMany(x => x.Blights);
            var validVms = (from id in validIds join vm in allVms on id equals vm.Id select vm).ToList();
            return validVms;
        }
    }
}
