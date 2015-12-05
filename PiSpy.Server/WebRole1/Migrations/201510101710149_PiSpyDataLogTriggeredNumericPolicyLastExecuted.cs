namespace WebRole1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PiSpyDataLogTriggeredNumericPolicyLastExecuted : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PolicyActions", "LastExecuted", c => c.DateTime());
            AddColumn("dbo.PiSpyDataLogs", "TriggeredNumericPolicy", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PiSpyDataLogs", "TriggeredNumericPolicy");
            DropColumn("dbo.PolicyActions", "LastExecuted");
        }
    }
}
