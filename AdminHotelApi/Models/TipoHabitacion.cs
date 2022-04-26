using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AdminHotelApi.Models
{
    [Table("TipoHabitaciones")]
    public class TipoHabitacion
    {
        [Key]
        public int TipoHabitacionId { get; set; }
        [Required]
        public string Nombre { get; set; }
        public int Capacidad { get; set; }
    }
}