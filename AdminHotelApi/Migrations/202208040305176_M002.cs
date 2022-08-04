namespace AdminHotelApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M002 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.TiposHabitacionesFotos");
            AddPrimaryKey("dbo.TiposHabitacionesFotos", new[] { "HotelId", "TipoHabitacionFotoId" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.TiposHabitacionesFotos");
            AddPrimaryKey("dbo.TiposHabitacionesFotos", new[] { "HotelId", "TipoHabitacionId", "TipoHabitacionFotoId" });
        }
    }
}
