using System.Threading.Tasks;

namespace FriendOrganizer.UI.Data.Repositories
{
    public interface IGenericRepository<T>
    {
        void Add(T model);
        Task<T> GetByIdAsync(int id);
        bool HasChanges();
        Task SaveAsync();
        void Remove(T model);
    }
}