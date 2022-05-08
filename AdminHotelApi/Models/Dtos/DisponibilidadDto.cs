using AdminHotelApi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminHotelApi.Models.Dtos
{
    public class DisponibilidadDto
    {
        public int TipoHabitacionId { get; set; }
        public TipoHabitacion TipoHabitacion { get; set; }
        public int Disponibles { get; set; }
        public IEnumerable<TarifaDto> Tarifas { get; set; }
    }
}