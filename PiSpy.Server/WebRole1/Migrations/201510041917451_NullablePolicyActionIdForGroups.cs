namespace WebRole1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NullablePolicyActionIdForGroups : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Policies", "PolicyActionId", "dbo.PolicyActions");
            DropIndex("dbo.Policies", new[] { "PolicyActionId" });
            AlterColumn("dbo.Policies", "PolicyActionId", c => c.Int());
            CreateIndex("dbo.Policies", "PolicyActionId");
            AddForeignKey("dbo.Policies", "PolicyActionId", "dbo.PolicyActions", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Policies", "PolicyActionId", "dbo.PolicyActions");
            DropIndex("dbo.Policies", new[] { "PolicyActionId" });
            AlterColumn("dbo.Policies", "PolicyActionId", c => c.Int(nullable: false));
            CreateIndex("dbo.Policies", "PolicyActionId");
            AddForeignKey("dbo.Policies", "PolicyActionId", "dbo.PolicyActions", "Id", cascadeDelete: true);
        }
    }
}
