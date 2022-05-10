using AdminHotelApi.Models;
using AdminHotelApi.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace AdminHotelApi.Data
{
    public class ReservacionDao
    {
        public static AdminHotelApiContext Db { get; set; }

        public static IEnumerable<DisponibilidadDto> GetDisponibilidades(DateTime fechaEntrada, DateTime fechaSalida)
        {
            StoredProcedure.Db = Db;
            IEnumerable<DisponibilidadDto> disponibilidades = 
                StoredProcedure.Execute<DisponibilidadDto>("sp_get_disponibilidades {0}, {1}, {2}, {3}", new object[] 
                { 
                    fechaEntrada,
                    fechaSalida,
                    0,
                    0
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