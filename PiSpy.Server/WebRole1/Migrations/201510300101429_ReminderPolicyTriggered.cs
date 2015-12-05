namespace WebRole1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReminderPolicyTriggered : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Policies", "Triggered", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Policies", "Triggered");
        }
    }
}
