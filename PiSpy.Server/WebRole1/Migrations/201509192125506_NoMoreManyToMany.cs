namespace WebRole1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NoMoreManyToMany : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ApplicationUserPiSpyDevices", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ApplicationUserPiSpyDevices", "PiSpyDevice_Id", "dbo.PiSpyDevices");
            DropIndex("dbo.ApplicationUserPiSpyDevices", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.ApplicationUserPiSpyDevices", new[] { "PiSpyDevice_Id" });
            AddColumn("dbo.PiSpyDevices", "ApplicationUser_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.PiSpyDevices", "ApplicationUser_Id");
            AddForeignKey("dbo.PiSpyDevices", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
            DropTable("dbo.ApplicationUserPiSpyDevices");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ApplicationUserPiSpyDevices",
                c => new
                    {
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                        PiSpyDevice_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ApplicationUser_Id, t.PiSpyDevice_Id });
            
            DropForeignKey("dbo.PiSpyDevices", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.PiSpyDevices", new[] { "ApplicationUser_Id" });
            DropColumn("dbo.PiSpyDevices", "ApplicationUser_Id");
            CreateIndex("dbo.ApplicationUserPiSpyDevices", "PiSpyDevice_Id");
            CreateIndex("dbo.ApplicationUserPiSpyDevices", "ApplicationUser_Id");
            AddForeignKey("dbo.ApplicationUserPiSpyDevices", "PiSpyDevice_Id", "dbo.PiSpyDevices", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ApplicationUserPiSpyDevices", "ApplicationUser_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
