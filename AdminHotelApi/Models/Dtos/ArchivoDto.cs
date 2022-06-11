using AdminHotelApi.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminHotelApi.Models.Dtos
{
    public class ArchivoDto : IArchivo
    {
        public string Nombre { get; set; }
        public int Tamanio { get { return Contenido != null ? Contenido.Length : 0; } }
        public byte[] Contenido { get; set; }
        public ArchivoDto()
        {

        }
    }
}