namespace AdminHotelApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M12052022 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TiposHabitacionesArchivos",
                c => new
                    {
                        HotelId = c.Int(nullable: false),
                        TipoHabitacionId = c.Int(nullable: false),
                        TipoHabitacionArchivoId = c.Int(nullable: false),
                        Nombre = c.String(nullable: false, maxLength: 128),
                        TipoContenido = c.String(maxLength: 32),
                        Contenido = c.Binary(maxLength: 2048),
                        Activo = c.Boolean(nullable: false),
                        FechaAlta = c.DateTime(nullable: false),
                        FechaUpdate = c.DateTime(),
                    })
                .PrimaryKey(t => new { t.HotelId, t.TipoHabitacionId, t.TipoHabitacionArchivoId })
                .ForeignKey("dbo.TiposHabitaciones", t => new { t.HotelId, t.TipoHabitacionId })
                .Index(t => new { t.HotelId, t.TipoHabitacionId });
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TiposHabitacionesArchivos", new[] { "HotelId", "TipoHabitacionId" }, "dbo.TiposHabitaciones");
            DropIndex("dbo.TiposHabitacionesArchivos", new[] { "HotelId", "TipoHabitacionId" });
            DropTable("dbo.TiposHabitacionesArchivos");
        }
    }
}
