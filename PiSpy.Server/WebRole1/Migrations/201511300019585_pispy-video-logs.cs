namespace WebRole1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pispyvideologs : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PiSpyVideoLogs",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TimeStamp = c.DateTime(nullable: false),
                        TimeReceived = c.DateTime(nullable: false),
                        IpAddress = c.String(),
                        FilePath = c.String(),
                        PiSpySerialNumber = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PiSpyVideoLogs");
        }
    }
}
