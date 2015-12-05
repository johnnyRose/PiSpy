namespace WebRole1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PolicyActions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PolicyActions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        EmailAddress = c.String(),
                        EmailMessage = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Policies", "PolicyActionId", c => c.Int(nullable: false));
            CreateIndex("dbo.Policies", "PolicyActionId");
            AddForeignKey("dbo.Policies", "PolicyActionId", "dbo.PolicyActions", "Id", cascadeDelete: true);
            DropColumn("dbo.Policies", "EmailAddress");
            DropColumn("dbo.Policies", "EmailMessage");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Policies", "EmailMessage", c => c.String());
            AddColumn("dbo.Policies", "EmailAddress", c => c.String());
            DropForeignKey("dbo.Policies", "PolicyActionId", "dbo.PolicyActions");
            DropIndex("dbo.Policies", new[] { "PolicyActionId" });
            DropColumn("dbo.Policies", "PolicyActionId");
            DropTable("dbo.PolicyActions");
        }
    }
}
