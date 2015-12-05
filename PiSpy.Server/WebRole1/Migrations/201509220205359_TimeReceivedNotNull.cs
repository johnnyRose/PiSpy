namespace WebRole1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TimeReceivedNotNull : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PiSpyDataLogs", "TimeReceived", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PiSpyDataLogs", "TimeReceived", c => c.DateTime());
        }
    }
}
