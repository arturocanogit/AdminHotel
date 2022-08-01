namespace AdminHotelApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M001 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Clientes",
                c => new
                    {
                        HotelId = c.Int(nullable: false),
                        ClienteId = c.Int(nullable: false),
                        Nombre = c.String(nullable: false, maxLength: 128),
                        Telefono = c.String(maxLength: 16),
                        Email = c.String(maxLength: 32),
                        Activo = c.Boolean(nullable: false),
                        FechaAlta = c.DateTime(nullable: false),
                        FechaUpdate = c.DateTime(),
                    })
                .PrimaryKey(t => new { t.HotelId, t.ClienteId })
                .ForeignKey("dbo.Hoteles", t => t.HotelId)
                .Index(t => t.HotelId);
            
            CreateTable(
                "dbo.Hoteles",
                c => new
                    {
                        HotelId = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 128),
                        Activo = c.Boolean(nullable: false),
                        FechaAlta = c.DateTime(nullable: false),
                        FechaUpdate = c.DateTime(),
                    })
                .PrimaryKey(t => t.HotelId);
            
            CreateTable(
                "dbo.Habitaciones",
                c => new
                    {
                        HotelId = c.Int(nullable: false),
                        HabitacionId = c.Int(nullable: false),
                        TipoHabitacionId = c.Int(nullable: false),
                        NumeroHabitacion = c.String(nullable: false, maxLength: 8),
                        Capacidad = c.Int(nullable: false),
                        Activo = c.Boolean(nullable: false),
                        FechaAlta = c.DateTime(nullable: false),
                        FechaUpdate = c.DateTime(),
                    })
                .PrimaryKey(t => new { t.HotelId, t.HabitacionId })
                .ForeignKey("dbo.TiposHabitaciones", t => new { t.HotelId, t.TipoHabitacionId })
                .Index(t => new { t.HotelId, t.TipoHabitacionId });
            
            CreateTable(
                "dbo.TiposHabitaciones",
                c => new
                    {
                        HotelId = c.Int(nullable: false),
                        TipoHabitacionId = c.Int(nullable: false),
                        Nombre = c.String(nullable: false, maxLength: 128),
                        Descripcion = c.String(maxLength: 256),
                        Activo = c.Boolean(nullable: false),
                        FechaAlta = c.DateTime(nullable: false),
                        FechaUpdate = c.DateTime(),
                    })
                .PrimaryKey(t => new { t.HotelId, t.TipoHabitacionId })
                .ForeignKey("dbo.Hoteles", t => t.HotelId)
                .Index(t => t.HotelId);
            
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
                        Desayuno = c.Boolean(nullable: false),
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
            
            CreateTable(
                "dbo.Reservaciones",
                c => new
                    {
                        HotelId = c.Int(nullable: false),
                        ReservacionId = c.Int(nullable: false),
                        ClienteId = c.Int(nullable: false),
                        Folio = c.String(nullable: false, maxLength: 64),
                        FechaEntrada = c.DateTime(nullable: false),
                        FechaSalida = c.DateTime(nullable: false),
                        Activo = c.Boolean(nullable: false),
                        FechaAlta = c.DateTime(nullable: false),
                        FechaUpdate = c.DateTime(),
                    })
                .PrimaryKey(t => new { t.HotelId, t.ReservacionId })
                .ForeignKey("dbo.Clientes", t => new { t.HotelId, t.ClienteId })
                .Index(t => new { t.HotelId, t.ClienteId });
            
            CreateTable(
                "dbo.Tarifas",
                c => new
                    {
                        HotelId = c.Int(nullable: false),
                        TarifaId = c.Int(nullable: false),
                        TipoHabitacionId = c.Int(nullable: false),
                        DiaSemanaId = c.Int(nullable: false),
                        Personas = c.Int(nullable: false),
                        Precio = c.Double(nullable: false),
                        Activo = c.Boolean(nullable: false),
                        FechaAlta = c.DateTime(nullable: false),
                        FechaUpdate = c.DateTime(),
                    })
                .PrimaryKey(t => new { t.HotelId, t.TarifaId })
                .ForeignKey("dbo.TiposHabitaciones", t => new { t.HotelId, t.TipoHabitacionId })
                .Index(t => new { t.HotelId, t.TipoHabitacionId });
            
            CreateTable(
                "dbo.TarifasFestivos",
                c => new
                    {
                        HotelId = c.Int(nullable: false),
                        TarifaId = c.Int(nullable: false),
                        TipoHabitacionId = c.Int(nullable: false),
                        Fecha = c.DateTime(nullable: false),
                        Personas = c.Int(nullable: false),
                        Precio = c.Double(nullable: false),
                        Activo = c.Boolean(nullable: false),
                        FechaAlta = c.DateTime(nullable: false),
                        FechaUpdate = c.DateTime(),
                    })
                .PrimaryKey(t => new { t.HotelId, t.TarifaId })
                .ForeignKey("dbo.TiposHabitaciones", t => new { t.HotelId, t.TipoHabitacionId })
                .Index(t => new { t.HotelId, t.TipoHabitacionId });
            
            CreateTable(
                "dbo.TiposHabitacionesFotos",
                c => new
                    {
                        HotelId = c.Int(nullable: false),
                        TipoHabitacionId = c.Int(nullable: false),
                        TipoHabitacionFotoId = c.Int(nullable: false),
                        Nombre = c.String(nullable: false, maxLength: 128),
                        TipoContenido = c.String(maxLength: 32),
                        Contenido = c.Binary(),
                        Activo = c.Boolean(nullable: false),
                        FechaAlta = c.DateTime(nullable: false),
                        FechaUpdate = c.DateTime(),
                    })
                .PrimaryKey(t => new { t.HotelId, t.TipoHabitacionId, t.TipoHabitacionFotoId })
                .ForeignKey("dbo.TiposHabitaciones", t => new { t.HotelId, t.TipoHabitacionId })
                .Index(t => new { t.HotelId, t.TipoHabitacionId });
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TiposHabitacionesFotos", new[] { "HotelId", "TipoHabitacionId" }, "dbo.TiposHabitaciones");
            DropForeignKey("dbo.TarifasFestivos", new[] { "HotelId", "TipoHabitacionId" }, "dbo.TiposHabitaciones");
            DropForeignKey("dbo.Tarifas", new[] { "HotelId", "TipoHabitacionId" }, "dbo.TiposHabitaciones");
            DropForeignKey("dbo.ReservacionDetalles", new[] { "HotelId", "TipoHabitacionId" }, "dbo.TiposHabitaciones");
            DropForeignKey("dbo.ReservacionDetalles", new[] { "HotelId", "ReservacionId" }, "dbo.Reservaciones");
            DropForeignKey("dbo.Reservaciones", new[] { "HotelId", "ClienteId" }, "dbo.Clientes");
            DropForeignKey("dbo.ReservacionDetalles", new[] { "HotelId", "HabitacionId" }, "dbo.Habitaciones");
            DropForeignKey("dbo.Habitaciones", new[] { "HotelId", "TipoHabitacionId" }, "dbo.TiposHabitaciones");
            DropForeignKey("dbo.TiposHabitaciones", "HotelId", "dbo.Hoteles");
            DropForeignKey("dbo.Clientes", "HotelId", "dbo.Hoteles");
            DropIndex("dbo.TiposHabitacionesFotos", new[] { "HotelId", "TipoHabitacionId" });
            DropIndex("dbo.TarifasFestivos", new[] { "HotelId", "TipoHabitacionId" });
            DropIndex("dbo.Tarifas", new[] { "HotelId", "TipoHabitacionId" });
            DropIndex("dbo.Reservaciones", new[] { "HotelId", "ClienteId" });
            DropIndex("dbo.ReservacionDetalles", new[] { "HotelId", "TipoHabitacionId" });
            DropIndex("dbo.ReservacionDetalles", new[] { "HotelId", "ReservacionId" });
            DropIndex("dbo.ReservacionDetalles", new[] { "HotelId", "HabitacionId" });
            DropIndex("dbo.TiposHabitaciones", new[] { "HotelId" });
            DropIndex("dbo.Habitaciones", new[] { "HotelId", "TipoHabitacionId" });
            DropIndex("dbo.Clientes", new[] { "HotelId" });
            DropTable("dbo.TiposHabitacionesFotos");
            DropTable("dbo.TarifasFestivos");
            DropTable("dbo.Tarifas");
            DropTable("dbo.Reservaciones");
            DropTable("dbo.ReservacionDetalles");
            DropTable("dbo.TiposHabitaciones");
            DropTable("dbo.Habitaciones");
            DropTable("dbo.Hoteles");
            DropTable("dbo.Clientes");
        }
    }
}
