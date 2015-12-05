namespace WebRole1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PolicyEngineTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Policies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ComparisonOperatorId = c.Int(),
                        Value = c.Double(),
                        NumericPolicyTypeId = c.Int(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ComparisonOperators", t => t.ComparisonOperatorId, cascadeDelete: true)
                .ForeignKey("dbo.NumericPolicyTypes", t => t.NumericPolicyTypeId, cascadeDelete: true)
                .Index(t => t.ComparisonOperatorId)
                .Index(t => t.NumericPolicyTypeId);
            
            CreateTable(
                "dbo.ComparisonOperators",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Operator = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.NumericPolicyTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PolicyGroupPolicies",
                c => new
                    {
                        PolicyGroup_Id = c.Int(nullable: false),
                        Policy_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PolicyGroup_Id, t.Policy_Id })
                .ForeignKey("dbo.Policies", t => t.PolicyGroup_Id)
                .ForeignKey("dbo.Policies", t => t.Policy_Id)
                .Index(t => t.PolicyGroup_Id)
                .Index(t => t.Policy_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Policies", "NumericPolicyTypeId", "dbo.NumericPolicyTypes");
            DropForeignKey("dbo.Policies", "ComparisonOperatorId", "dbo.ComparisonOperators");
            DropForeignKey("dbo.PolicyGroupPolicies", "Policy_Id", "dbo.Policies");
            DropForeignKey("dbo.PolicyGroupPolicies", "PolicyGroup_Id", "dbo.Policies");
            DropIndex("dbo.PolicyGroupPolicies", new[] { "Policy_Id" });
            DropIndex("dbo.PolicyGroupPolicies", new[] { "PolicyGroup_Id" });
            DropIndex("dbo.Policies", new[] { "NumericPolicyTypeId" });
            DropIndex("dbo.Policies", new[] { "ComparisonOperatorId" });
            DropTable("dbo.PolicyGroupPolicies");
            DropTable("dbo.NumericPolicyTypes");
            DropTable("dbo.ComparisonOperators");
            DropTable("dbo.Policies");
        }
    }
}
