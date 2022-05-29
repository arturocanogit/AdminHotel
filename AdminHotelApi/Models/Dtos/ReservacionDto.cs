using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminHotelApi.Models.Dtos
{
    public class ReservacionDto
    {
        public int TipoHabitacionId { get; set; }
        public int Personas { get; set; }
        public int Precio { get; set; }
    }
}