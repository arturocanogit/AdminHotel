using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminHotelApi.Models.Interfaces
{
    public interface IArchivo
    {
        string Nombre { get; set; }
        int Tamanio { get; set; }
        byte[] Contenido { get; set; }
    }
}