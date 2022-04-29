using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AdminHotelApi.Models
{
    [Table("Reservaciones")]
    public class Reservacion : Entity
    {
        [Key, Column(Order = 0)]
        public int HotelId { get; set; }
        [Key, Column(Order = 1)]
        public int ReservacionId { get; set; }
        public int ClienteId { get; set; }
        public int HabitacionId { get; set; }
        public int TipoHabitacionId { get; set; }
        public virtual Cliente Cliente { get; set; }
        public virtual Habitacion Habitacion { get; set; }
        public virtual TipoHabitacion TipoHabitacion { get; set; }
    }
}