using FriendOrganizer.UI.Event;
using Prism.Events;
using System;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private IEventAggregator _eventAggregator;
        private IFriendDetailViewModel _friendDetailViewModel;

        private Func<IFriendDetailViewModel> _friendDetailViewModelCreator;

        public IFriendDetailViewModel FriendDetailViewModel
        {
            get { return _friendDetailViewModel;}
            private set
            {
                _friendDetailViewModel = value;
                OnPropertyChanged();
            }
        }

        public INavigationViewModel NavigationViewModel { get; }

        public MainViewModel(INavigationViewModel navigationViewModel, Func<IFriendDetailViewModel> friendDetailViewModelCreator, IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _friendDetailViewModelCreator = friendDetailViewModelCreator;

            _eventAggregator.GetEvent<OpenFriendDetailViewEvent>()
                .Subscribe(OnOpenFriendDetailView);

            NavigationViewModel = navigationViewModel;
        }

        public async Task LoadAsync()
        {
            await NavigationViewModel.LoadAsync();
        }

        private async void OnOpenFriendDetailView(int friendId)
        {
            FriendDetailViewModel = _friendDetailViewModelCreator();
            await FriendDetailViewModel.LoadAsync(friendId);
        }
    }
}
