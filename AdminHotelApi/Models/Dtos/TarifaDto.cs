using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminHotelApi.Models.Dtos
{
    public class TarifaDto
    {
        public int HotelId { get; set; }
        public int tipoHabitacionId { get; set; }
        public int TarifaId { get; set; }
        public int DiaSemanaId { get; set; }
        public int Personas { get; set; }
        public double Precio { get; set; }
    }
}