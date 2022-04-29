using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AdminHotelApi.Models
{
    [Table("Habitaciones")]
    public class Habitacion : Entity
    {
        [Key, Column(Order = 0)]
        public int HotelId { get; set; }
        [Key, Column(Order = 1)]
        public int HabitacionId { get; set; }
        public int TipoHabitacionId { get; set; }
        [Required, MaxLength(8)]
        public string NumeroHabitacion { get; set; }
        public virtual TipoHabitacion TipoHabitacion { get; set; }
    }
}