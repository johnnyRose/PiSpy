namespace WebRole1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PiSpyDataLogTimeReceived : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PiSpyDataLogs", "TimeReceived", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PiSpyDataLogs", "TimeReceived");
        }
    }
}
