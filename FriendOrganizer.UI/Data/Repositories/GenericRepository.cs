using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.Data.Repositories
{
    public class GenericRepository<TEntity, TContext> :
        IGenericRepository<TEntity> where TContext : DbContext
        where TEntity : class
    {
        public void Add(TEntity model)
        {
            Context.Set<TEntity>().Add(model);
        }

        protected readonly TContext Context;

        public virtual async Task<TEntity> GetByIdAsync(int id)
        {
            return await Context.Set<TEntity>().FindAsync(id);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await Context.Set<TEntity>().ToListAsync();
        }

        public bool HasChanges()
        {
            return Context.ChangeTracker.HasChanges();
        }

        protected GenericRepository(TContext context)
        {
            this.Context = context;
        }

        public void Remove(TEntity model)
        {
            Context.Set<TEntity>().Remove(model);
        }

        public async Task SaveAsync()
        {
            await Context.SaveChangesAsync();
        }
    }
}
