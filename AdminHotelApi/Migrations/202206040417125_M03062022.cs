namespace AdminHotelApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M03062022 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Reservaciones", new[] { "HotelId", "HabitacionId" }, "dbo.Habitaciones");
            DropForeignKey("dbo.Reservaciones", new[] { "HotelId", "TipoHabitacionId" }, "dbo.TiposHabitaciones");
            DropIndex("dbo.Reservaciones", new[] { "HotelId", "HabitacionId" });
            DropIndex("dbo.Reservaciones", new[] { "HotelId", "TipoHabitacionId" });
            CreateTable(
                "dbo.ReservacionDetalles",
                c => new
                    {
                        HotelId = c.Int(nullable: false),
                        ReservacionId = c.Int(nullable: false),
                        ReservacionDetalleId = c.Int(nullable: false),
                        HabitacionId = c.Int(),
                        TipoHabitacionId = c.Int(nullable: false),
                        Personas = c.Int(nullable: false),
                        Precio = c.Int(nullable: false),
                        Activo = c.Boolean(nullable: false),
                        FechaAlta = c.DateTime(nullable: false),
                        FechaUpdate = c.DateTime(),
                    })
                .PrimaryKey(t => new { t.HotelId, t.ReservacionId, t.ReservacionDetalleId })
                .ForeignKey("dbo.Habitaciones", t => new { t.HotelId, t.HabitacionId })
                .ForeignKey("dbo.Reservaciones", t => new { t.HotelId, t.ReservacionId })
                .ForeignKey("dbo.TiposHabitaciones", t => new { t.HotelId, t.TipoHabitacionId })
                .Index(t => new { t.HotelId, t.HabitacionId })
                .Index(t => new { t.HotelId, t.ReservacionId })
                .Index(t => new { t.HotelId, t.TipoHabitacionId });
            
            DropColumn("dbo.Reservaciones", "HabitacionId");
            DropColumn("dbo.Reservaciones", "TipoHabitacionId");
            DropColumn("dbo.Reservaciones", "Personas");
            DropColumn("dbo.Reservaciones", "Precio");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Reservaciones", "Precio", c => c.Int(nullable: false));
            AddColumn("dbo.Reservaciones", "Personas", c => c.Int(nullable: false));
            AddColumn("dbo.Reservaciones", "TipoHabitacionId", c => c.Int(nullable: false));
            AddColumn("dbo.Reservaciones", "HabitacionId", c => c.Int());
            DropForeignKey("dbo.ReservacionDetalles", new[] { "HotelId", "TipoHabitacionId" }, "dbo.TiposHabitaciones");
            DropForeignKey("dbo.ReservacionDetalles", new[] { "HotelId", "ReservacionId" }, "dbo.Reservaciones");
            DropForeignKey("dbo.ReservacionDetalles", new[] { "HotelId", "HabitacionId" }, "dbo.Habitaciones");
            DropIndex("dbo.ReservacionDetalles", new[] { "HotelId", "TipoHabitacionId" });
            DropIndex("dbo.ReservacionDetalles", new[] { "HotelId", "ReservacionId" });
            DropIndex("dbo.ReservacionDetalles", new[] { "HotelId", "HabitacionId" });
            DropTable("dbo.ReservacionDetalles");
            CreateIndex("dbo.Reservaciones", new[] { "HotelId", "TipoHabitacionId" });
            CreateIndex("dbo.Reservaciones", new[] { "HotelId", "HabitacionId" });
            AddForeignKey("dbo.Reservaciones", new[] { "HotelId", "TipoHabitacionId" }, "dbo.TiposHabitaciones", new[] { "HotelId", "TipoHabitacionId" });
            AddForeignKey("dbo.Reservaciones", new[] { "HotelId", "HabitacionId" }, "dbo.Habitaciones", new[] { "HotelId", "HabitacionId" });
        }
    }
}
