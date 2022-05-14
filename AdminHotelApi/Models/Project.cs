using AdminHotelApi.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminHotelApi.Models
{
    public class Project
    {
        public static ArchivoDto PostedFileToDto(HttpPostedFileBase file)
        {
            return new ArchivoDto
            {
                Nombre = file.FileName,
                Tamanio = file.ContentLength,
                Contenido = Utilerias.StreamToByteArray(file.InputStream)
            };
        }
    }
}