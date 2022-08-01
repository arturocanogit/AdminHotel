using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminHotelApi.Models.Dtos
{
    public class TarifaDto
    {
        public int HotelId { get; set; }
        public int TarifaId { get; set; }
        public int TipoHabitacionId { get; set; }
        public int DiaSemanaId { get; set; }
        public string DiaSemana { get { return Enum.GetName(typeof(DiasDeLaSemana), DiaSemanaId); } }
        public int Personas { get; set; }
        public double Precio { get; set; }
        public HotelDto Hotel { get; set; }
        public TipoHabitacionDto TipoHabitacion { get; set; }
    }
}