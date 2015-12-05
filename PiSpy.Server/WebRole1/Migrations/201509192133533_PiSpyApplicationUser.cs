namespace WebRole1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PiSpyApplicationUser : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.PiSpyDevices", name: "ApplicationUser_Id", newName: "ApplicationUserId");
            RenameIndex(table: "dbo.PiSpyDevices", name: "IX_ApplicationUser_Id", newName: "IX_ApplicationUserId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.PiSpyDevices", name: "IX_ApplicationUserId", newName: "IX_ApplicationUser_Id");
            RenameColumn(table: "dbo.PiSpyDevices", name: "ApplicationUserId", newName: "ApplicationUser_Id");
        }
    }
}
