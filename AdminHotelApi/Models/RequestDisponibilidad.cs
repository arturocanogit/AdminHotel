using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminHotelApi.Models
{
    public class RequestDisponibilidad
    {
        public DateTime FechaEntrada { get; set; }
        public DateTime FechaSalida { get; set; }
        public int Adultos { get; set; }
        public int Menores { get; set; }
    }
}