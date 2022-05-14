using AdminHotelApi.Models;
using AdminHotelApi.Models.Dtos;
using AdminHotelApi.Models.Entities;
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
                item.TipoHabitacion = Utilerias.Mapeador<TipoHabitacionDto, TipoHabitacion>(
                    Db.TiposHabitaciones.First(x => x.TipoHabitacionId == item.TipoHabitacionId));

                item.TipoHabitacion.DownloadFoto = Db.TiposHabitacionesFotos
                    .FirstOrDefault(x =>
                    x.HotelId == item.TipoHabitacion.HotelId &&
                    x.TipoHabitacionId == item.TipoHabitacion.TipoHabitacionId);
            }
            return disponibilidades;
        }
    }
}