namespace WebRole1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PiSpyDataLogPiSpySerialNumber : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PiSpyDataLogs", "PiSpyDeviceId", "dbo.PiSpyDevices");
            DropIndex("dbo.PiSpyDataLogs", new[] { "PiSpyDeviceId" });
            AddColumn("dbo.PiSpyDataLogs", "PiSpySerialNumber", c => c.String());
            DropColumn("dbo.PiSpyDataLogs", "PiSpyDeviceId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PiSpyDataLogs", "PiSpyDeviceId", c => c.Int(nullable: false));
            DropColumn("dbo.PiSpyDataLogs", "PiSpySerialNumber");
            CreateIndex("dbo.PiSpyDataLogs", "PiSpyDeviceId");
            AddForeignKey("dbo.PiSpyDataLogs", "PiSpyDeviceId", "dbo.PiSpyDevices", "Id", cascadeDelete: true);
        }
    }
}
