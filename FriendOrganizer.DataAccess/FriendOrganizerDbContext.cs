using FriendOrganizer.Model;
using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace FriendOrganizer.DataAccess
{
    public class FriendOrganizerDbContext : DbContext
    {
        public FriendOrganizerDbContext():base("FriendOrganizerDb")
        {

        }

        public DbSet<FriendPhoneNumber> FriendPhoneNumbers { get; set; }

        public DbSet<Friend> Friends { get; set; }

        public DbSet<Meeting> Meetings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
          base.OnModelCreating(modelBuilder);
          modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public DbSet<ProgrammingLanguage> ProgrammingLanguages { get; set; }
    }
}
