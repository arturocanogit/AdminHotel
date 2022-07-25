using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminHotelApi.Models.Dtos
{
    public class HabitacionDto
    {
        public int HotelId { get; set; }
        public int HabitacionId { get; set; }
        public int TipoHabitacionId { get; set; }
        public string NumeroHabitacion { get; set; }
        public int Capacidad { get; set; }
    }
}