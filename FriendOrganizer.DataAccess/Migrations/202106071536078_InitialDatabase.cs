using System;
using System.Data.Entity.Migrations;

namespace FriendOrganizer.DataAccess.Migrations
{
    public partial class InitialDatabase : DbMigration
    {
        public override void Down()
        {
            AlterColumn("dbo.Friend", "Email", c => c.String(nullable: false));
            AlterColumn("dbo.Friend", "LastName", c => c.String(nullable: false));
        }

        public override void Up()
        {
            AlterColumn("dbo.Friend", "LastName", c => c.String(maxLength: 50));
            AlterColumn("dbo.Friend", "Email", c => c.String(maxLength: 50));
        }
    }
}
