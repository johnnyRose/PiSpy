namespace WebRole1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReminderAndDisconnectPolicies : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Policies", "MinutesDisconnected", c => c.Int());
            AddColumn("dbo.Policies", "TriggerTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Policies", "TriggerTime");
            DropColumn("dbo.Policies", "MinutesDisconnected");
        }
    }
}
