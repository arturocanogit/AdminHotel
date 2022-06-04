using AdminHotelApi.Models;
using AdminHotelApi.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace AdminHotelApi.Data
{
    public class AdminHotelApiContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public AdminHotelApiContext() : base("name=AdminHotelApiContext")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }

        public DbSet<Hotel> Hoteles { get; set; }
        public DbSet<TipoHabitacion> TiposHabitaciones { get; set; }
        public DbSet<Habitacion> Habitaciones { get; set; }
        public DbSet<Tarifa> Tarifas { get; set; }
        public DbSet<TarifaFestivo> TarifasFestivos { get; set; }
        public DbSet<Reservacion> Reservaciones { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<TipoHabitacionFoto> TiposHabitacionesFotos { get; set; }

        public System.Data.Entity.DbSet<AdminHotelApi.Models.Entities.ReservacionDetalle> ReservacionDetalles { get; set; }
    }
}
