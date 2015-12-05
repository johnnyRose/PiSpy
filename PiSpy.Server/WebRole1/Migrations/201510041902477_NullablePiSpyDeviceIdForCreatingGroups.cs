namespace WebRole1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NullablePiSpyDeviceIdForCreatingGroups : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Policies", "PiSpyDeviceId", "dbo.PiSpyDevices");
            DropIndex("dbo.Policies", new[] { "PiSpyDeviceId" });
            AlterColumn("dbo.Policies", "PiSpyDeviceId", c => c.Int());
            CreateIndex("dbo.Policies", "PiSpyDeviceId");
            AddForeignKey("dbo.Policies", "PiSpyDeviceId", "dbo.PiSpyDevices", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Policies", "PiSpyDeviceId", "dbo.PiSpyDevices");
            DropIndex("dbo.Policies", new[] { "PiSpyDeviceId" });
            AlterColumn("dbo.Policies", "PiSpyDeviceId", c => c.Int(nullable: false));
            CreateIndex("dbo.Policies", "PiSpyDeviceId");
            AddForeignKey("dbo.Policies", "PiSpyDeviceId", "dbo.PiSpyDevices", "Id", cascadeDelete: true);
        }
    }
}
