using FriendOrganizer.Model;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.ViewModel
{
    public interface INavigationViewModel
    {
        Task LoadAsync();
    }
}