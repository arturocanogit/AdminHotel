using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminHotelApi.Models.Dtos
{
    public class ResultadoDto
    {
        public string Mensaje { get; set; }
    }

    public class ResultadoDto<T>
    {
        public string Mensaje { get; set; }
        public T Datos { get; set; }
    }
}