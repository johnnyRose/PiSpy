namespace WebRole1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PiSpyDataLogs : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PiSpyDataLogs",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TimeStamp = c.DateTime(nullable: false),
                        IpAddress = c.String(),
                        Temperature = c.Double(nullable: false),
                        Humidity = c.Double(nullable: false),
                        PiSpyDeviceId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PiSpyDevices", t => t.PiSpyDeviceId, cascadeDelete: true)
                .Index(t => t.PiSpyDeviceId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PiSpyDataLogs", "PiSpyDeviceId", "dbo.PiSpyDevices");
            DropIndex("dbo.PiSpyDataLogs", new[] { "PiSpyDeviceId" });
            DropTable("dbo.PiSpyDataLogs");
        }
    }
}
