namespace AdminHotelApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration_29052022_1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reservaciones", "Folio", c => c.String(nullable: false, maxLength: 64));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Reservaciones", "Folio");
        }
    }
}
