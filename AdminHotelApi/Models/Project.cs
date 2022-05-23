using AdminHotelApi.Models.Dtos;
using Global;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace AdminHotelApi.Models
{
    public static class Project
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