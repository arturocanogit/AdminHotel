using AdminHotelApi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace AdminHotelApi.Data
{
    public class ReservacionDal
    {
        public static AdminHotelApiContext Db { get; set; }

        public static IEnumerable<Disponibilidad> GetDisponibilidades(RequestDisponibilidad request)
        {
            StoredProcedure.Db = Db;
            IEnumerable<Disponibilidad> disponibilidades = 
                StoredProcedure.Execute<Disponibilidad>("sp_get_disponibilidades {0}, {1}, {2}, {3}", new object[] 
                { 
                    request.FechaEntrada, 
                    request.FechaSalida, 
                    request.Adultos, 
                    request.Adultos 
                });
            foreach (var item in disponibilidades)
            {
                item.TipoHabitacion = Db.TiposHabitaciones
                    .First(x => x.TipoHabitacionId == item.TipoHabitacionId);
            }
            return disponibilidades;
        }
    }
}