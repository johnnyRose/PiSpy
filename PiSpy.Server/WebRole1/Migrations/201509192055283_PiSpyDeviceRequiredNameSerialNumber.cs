namespace WebRole1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PiSpyDeviceRequiredNameSerialNumber : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PiSpyDevices", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.PiSpyDevices", "SerialNumber", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PiSpyDevices", "SerialNumber", c => c.String());
            AlterColumn("dbo.PiSpyDevices", "Name", c => c.String());
        }
    }
}
