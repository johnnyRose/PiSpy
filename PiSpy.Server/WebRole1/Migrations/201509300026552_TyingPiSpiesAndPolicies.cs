namespace WebRole1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TyingPiSpiesAndPolicies : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Policies", "PiSpyDeviceId", c => c.Int(nullable: false));
            AddColumn("dbo.Policies", "EmailAddress", c => c.String());
            AddColumn("dbo.Policies", "EmailMessage", c => c.String());
            CreateIndex("dbo.Policies", "PiSpyDeviceId");
            AddForeignKey("dbo.Policies", "PiSpyDeviceId", "dbo.PiSpyDevices", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Policies", "PiSpyDeviceId", "dbo.PiSpyDevices");
            DropIndex("dbo.Policies", new[] { "PiSpyDeviceId" });
            DropColumn("dbo.Policies", "EmailMessage");
            DropColumn("dbo.Policies", "EmailAddress");
            DropColumn("dbo.Policies", "PiSpyDeviceId");
        }
    }
}
