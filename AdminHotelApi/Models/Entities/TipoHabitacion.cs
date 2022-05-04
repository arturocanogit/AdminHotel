using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AdminHotelApi.Models
{
    [Table("TiposHabitaciones")]
    public class TipoHabitacion : Entity
    {
        [Key, Column(Order = 0)]
        public int HotelId { get; set; }
        [Key, Column(Order = 1)]
        public int TipoHabitacionId { get; set; }
        [Required, MaxLength(128)]
        public string Nombre { get; set; }
        [MaxLength(256)]
        public string Descripcion { get; set; }
        public virtual Hotel Hotel { get; set; }
    }
}