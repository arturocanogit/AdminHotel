using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AdminHotelApi.Models
{
    [Table("Reservaciones")]
    public class Reservacion
    {
        [Key]
        public int ReservacionId { get; set; }
        public int ClienteId { get; set; }
        public int HabitacionId { get; set; }
        public virtual Cliente Cliente { get; set; }
        public virtual Habitacion Habitacion { get; set; }
    }
}