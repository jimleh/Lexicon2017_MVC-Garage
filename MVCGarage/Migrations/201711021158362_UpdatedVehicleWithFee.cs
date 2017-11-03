namespace MVCGarage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedVehicleWithFee : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Vehicles", "Fee", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Vehicles", "Fee");
        }
    }
}
