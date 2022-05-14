using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AdminHotelApi.Models.Entities
{
    [Table("TiposHabitacionesFotos")]
    public class TipoHabitacionFoto : Entity
    {
        [Key, Column(Order = 0)]
        public int HotelId { get; set; }
        [Key, Column(Order = 1)]
        public int TipoHabitacionId { get; set; }
        [Key, Column(Order = 2)]
        public int TipoHabitacionArchivoId { get; set; }
        [Required, MaxLength(128)]
        public string Nombre { get; set; }
        [MaxLength(32)]
        public string TipoContenido { get; set; }
        [MaxLength(2048)]
        public byte[] Contenido { get; set; }
        public virtual TipoHabitacion TipoHabitacion { get; set; }
    }
}