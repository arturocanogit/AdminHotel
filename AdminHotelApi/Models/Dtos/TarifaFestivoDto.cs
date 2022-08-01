using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminHotelApi.Models.Dtos
{
    public class TarifaFestivoDto
    {
        public int HotelId { get; set; }
        public int TarifaId { get; set; }
        public int TipoHabitacionId { get; set; }
        public DateTime Fecha { get; set; }
        public int Personas { get; set; }
        public double Precio { get; set; }
        public HotelDto Hotel { get; set; }
        public TipoHabitacionDto TipoHabitacion { get; set; }
    }
}