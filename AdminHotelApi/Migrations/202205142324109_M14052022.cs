namespace AdminHotelApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M14052022 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.TiposHabitacionesFotos");
            AddColumn("dbo.TiposHabitacionesFotos", "TipoHabitacionFotoId", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.TiposHabitacionesFotos", new[] { "HotelId", "TipoHabitacionId", "TipoHabitacionFotoId" });
            DropColumn("dbo.TiposHabitacionesFotos", "TipoHabitacionArchivoId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TiposHabitacionesFotos", "TipoHabitacionArchivoId", c => c.Int(nullable: false));
            DropPrimaryKey("dbo.TiposHabitacionesFotos");
            DropColumn("dbo.TiposHabitacionesFotos", "TipoHabitacionFotoId");
            AddPrimaryKey("dbo.TiposHabitacionesFotos", new[] { "HotelId", "TipoHabitacionId", "TipoHabitacionArchivoId" });
        }
    }
}
