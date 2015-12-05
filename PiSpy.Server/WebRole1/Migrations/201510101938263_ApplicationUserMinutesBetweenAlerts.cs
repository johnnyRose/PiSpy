namespace WebRole1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApplicationUserMinutesBetweenAlerts : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "MinutesBetweenAlerts", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "MinutesBetweenAlerts");
        }
    }
}
