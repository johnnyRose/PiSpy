namespace WebRole1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NumericPolicyTypePostUnit : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NumericPolicyTypes", "PostUnit", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.NumericPolicyTypes", "PostUnit");
        }
    }
}
