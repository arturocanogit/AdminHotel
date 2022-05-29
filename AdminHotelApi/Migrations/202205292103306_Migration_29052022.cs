namespace AdminHotelApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration_29052022 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reservaciones", "Personas", c => c.Int(nullable: false));
            AddColumn("dbo.Reservaciones", "Precio", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Reservaciones", "Precio");
            DropColumn("dbo.Reservaciones", "Personas");
        }
    }
}
