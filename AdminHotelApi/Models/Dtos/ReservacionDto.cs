using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminHotelApi.Models.Dtos
{
    public class ReservacionDto
    {
        public string Folio { get; set; }
        public DateTime FechaEntrada { get; set; }
        public DateTime FechaSalida { get; set; }
        public DateTime FechaAlta { get; set; }
        public ClienteDto Cliente { get; set; }
        public List<ReservacionDetalleDto> Resevaciones { get; set; }

    }
}