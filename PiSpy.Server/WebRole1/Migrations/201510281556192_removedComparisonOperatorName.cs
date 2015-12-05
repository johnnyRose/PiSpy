namespace WebRole1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removedComparisonOperatorName : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ComparisonOperators", "Name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ComparisonOperators", "Name", c => c.String());
        }
    }
}
