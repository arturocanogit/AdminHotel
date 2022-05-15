using AdminHotelApi.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminHotelApi.Models.Dtos
{
    public class TipoHabitacionDto 
    {
        public int HotelId { get; set; }
        public int TipoHabitacionId { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public HttpPostedFileBase UploadFoto { get; set; }
        public ArchivoDto Foto { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaAlta { get; set; }
    }
}