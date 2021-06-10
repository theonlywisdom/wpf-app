using FriendOrganizer.Model;
using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace FriendOrganizer.DataAccess
{
    public class FriendOrganizerDbContext:DbContext
    {
        public DbSet<Friend> Friends { get; set; }

        public FriendOrganizerDbContext():base("FriendOrganizerDb")
        {

        }

        public static implicit operator Func<object>(FriendOrganizerDbContext v)
        {
            throw new NotImplementedException();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
          base.OnModelCreating(modelBuilder);
          modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
