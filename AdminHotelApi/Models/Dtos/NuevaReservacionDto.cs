using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminHotelApi.Models.Dtos
{
    public class NuevaReservacionDto
    {
        public DateTime FechaEntrada { get; set; }
        public DateTime FechaSalida { get; set; }
        public Cliente Cliente { get; set; }
        public IEnumerable<ReservacionDto> Resevaciones { get; set; }
    }
}