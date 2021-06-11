using FriendOrganizer.Model;
using FriendOrganizer.UI.Data.Lookups;
using FriendOrganizer.UI.Data.Repositories;
using FriendOrganizer.UI.Event;
using FriendOrganizer.UI.View.Services;
using FriendOrganizer.UI.Wrapper;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FriendOrganizer.UI.ViewModel
{
    public class FriendDetailViewModel : ViewModelBase, IFriendDetailViewModel
    {
        private IEventAggregator _eventAggregator;
        private FriendWrapper _friend;
        private IFriendRepository _friendRepository;
        private bool _hasChanges;
        private IMessageDialogService _messageDialogService;
        private IProgrammingLanguageLookupDataService _programmingLanguageLookupDataService;
        private FriendPhoneNumberWrapper _selectedPhoneNumber;

        public ICommand AddPhoneNumberCommand { get; }

        private Friend CreateNewFriend()
        {
            var friend = new Friend();
            _friendRepository.Add(friend);
            return friend;
        }

        public ICommand DeleteCommand { get; }

        public FriendWrapper Friend
        {
            get { return _friend; }
            set
            {
                _friend = value;
                OnPropertyChanged();
            }
        }

        public FriendDetailViewModel(IFriendRepository friendRepository, IEventAggregator eventAggregator, IMessageDialogService messageDialogService, IProgrammingLanguageLookupDataService programmingLanguageLookupDataService)
        {
            _friendRepository = friendRepository;
            _eventAggregator = eventAggregator;
            _messageDialogService = messageDialogService;
            _programmingLanguageLookupDataService = programmingLanguageLookupDataService;

            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
            DeleteCommand = new DelegateCommand(OnDeleteExecute);
            AddPhoneNumberCommand = new DelegateCommand(OnAddPhoneNumberExecute);
            RemovePhoneNumberCommand = new DelegateCommand(RemovePhoneNumberExecute, OnRemovePhoneNumberCanExecute);

            ProgrammingLanguages = new ObservableCollection<LookupItem>();
            PhoneNumbers = new ObservableCollection<FriendPhoneNumberWrapper>();
        }

        private bool OnRemovePhoneNumberCanExecute()
        {
            return SelectedPhoneNumber != null;
        }

        private void RemovePhoneNumberExecute()
        {
            // TODO: implement this

        }

        private void OnAddPhoneNumberExecute()
        {
            var newNumber = new FriendPhoneNumberWrapper(new FriendPhoneNumber());
            newNumber.PropertyChanged += FriendPhoneNumberWrapper_PropertyChanged;
            PhoneNumbers.Add(newNumber);
            Friend.Model.PhoneNumbers.Add(newNumber.Model);
            newNumber.Number = ""; // Trigger validation
        }

        private async void OnDeleteExecute()
        {
            var result = _messageDialogService.ShowOkCancelDialog($"Do you really want to delete the {Friend.FirstName} {Friend.LastName}", "Question");
            if (result == MessageDialogResult.OK) 
            {
                _friendRepository.Remove(Friend.Model);
                await _friendRepository.SaveAsync();
                _eventAggregator.GetEvent<AfterFriendDeletedEvent>().Publish(Friend.Id);
            }
        }

        public bool HasChanges
        {
            get { return _hasChanges; }
            set 
            {   if (_hasChanges != value) 
                { 
                    _hasChanges = value;
                    OnPropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public async Task LoadAsync(int? friendId)
        {
            var friend = friendId.HasValue
                ? await _friendRepository.GetByIdAsync(friendId.Value)
                : CreateNewFriend();

            InitializeFriend(friend);
            InitializeFriendPhoneNumbers(friend.PhoneNumbers);

            await LoadProgrammingLanguagesLookupAsync();
        }

        private void InitializeFriendPhoneNumbers(ICollection<FriendPhoneNumber> phoneNumbers)
        {
            foreach (var wrapper in PhoneNumbers)
            {
                wrapper.PropertyChanged -= FriendPhoneNumberWrapper_PropertyChanged;
            }
            PhoneNumbers.Clear();
            foreach (var friendPhoneNumber in phoneNumbers)
            {
                var wrapper = new FriendPhoneNumberWrapper(friendPhoneNumber);
                PhoneNumbers.Add(wrapper);
                wrapper.PropertyChanged += FriendPhoneNumberWrapper_PropertyChanged;
            }
        }

        private void FriendPhoneNumberWrapper_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!HasChanges)
            {
                HasChanges = _friendRepository.HasChanges();
            }
            if (e.PropertyName == nameof(FriendPhoneNumberWrapper.HasErrors))
            {
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

        private void InitializeFriend(Friend friend)
        {
            Friend = new FriendWrapper(friend);
            Friend.PropertyChanged += (s, e) =>
            {
                if (!HasChanges)
                {
                    HasChanges = _friendRepository.HasChanges();
                }
                if (e.PropertyName == nameof(Friend.HasErrors))
                {
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            };
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            if (Friend.Id == 0)
            {
                // A little trick to trigger the validation.
                Friend.FirstName = "";
            }
        }

        private async Task LoadProgrammingLanguagesLookupAsync()
        {
            ProgrammingLanguages.Clear();
            ProgrammingLanguages.Add(new NullLookupItem { DisplayMember = " - "});
            var lookup = await _programmingLanguageLookupDataService.GetProgrammingLanguageLookupAsync();
            foreach (var lookupItem in lookup)
            {
                ProgrammingLanguages.Add(lookupItem);
            }
        }

        private bool OnSaveCanExecute()
        {
            return Friend != null 
                && !Friend.HasErrors
                && PhoneNumbers.All(pn => !pn.HasErrors)
                && HasChanges;
        }

        private async void OnSaveExecute()
        {
            await _friendRepository.SaveAsync();
            HasChanges = _friendRepository.HasChanges();
            _eventAggregator.GetEvent<AfterFriendSavedEvent>().Publish(
                new AfterFriendSavedEventArgs
                {
                    Id = Friend.Id,
                    DisplayMember = $"{Friend.FirstName} {Friend.LastName}"
                });
        }

        public ObservableCollection<FriendPhoneNumberWrapper> PhoneNumbers { get; }

        public ObservableCollection<LookupItem> ProgrammingLanguages { get; }

        public ICommand RemovePhoneNumberCommand { get; }

        public ICommand SaveCommand { get; }

        public FriendPhoneNumberWrapper SelectedPhoneNumber 
        {
            get { return _selectedPhoneNumber; }
            set 
            {
                _selectedPhoneNumber = value;
                OnPropertyChanged();
                ((DelegateCommand)RemovePhoneNumberCommand).RaiseCanExecuteChanged();
            }
        }
    }
}
