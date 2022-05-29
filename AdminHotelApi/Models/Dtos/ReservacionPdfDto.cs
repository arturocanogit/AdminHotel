using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminHotelApi.Models.Dtos
{
    public class ReservacionPdfDto
    {
        public string FolioReservacion { get; set; }
        public ArchivoDto Archivo { get; set; }
    }
}