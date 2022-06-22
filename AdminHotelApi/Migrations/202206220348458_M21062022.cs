namespace AdminHotelApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M21062022 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReservacionDetalles", "Desayuno", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ReservacionDetalles", "Desayuno");
        }
    }
}
