using System.Threading.Tasks;
using FriendOrganizer.Model;

namespace FriendOrganizer.UI.Data.Repositories
{
    public interface IFriendRepository
  {
        void Add(Friend friend);
        Task<Friend> GetByIdAsync(int friendId);
        bool HasChanges();
        Task SaveAsync();
    }
}