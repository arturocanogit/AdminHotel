using Newtonsoft.Json;
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
        public int? HabitacionId { get; set; }
        public int TipoHabitacionId { get; set; }
        public int Personas { get; set; }
        public int Precio { get; set; }
        public DateTime FechaEntrada { get; set; }
        public DateTime FechaSalida { get; set; }
        public virtual Cliente Cliente { get; set; }
        [JsonIgnore]
        public virtual Habitacion Habitacion { get; set; }
        [JsonIgnore]
        public virtual TipoHabitacion TipoHabitacion { get; set; }
        [Required, MaxLength(64)]
        public string Folio { get; set; }
    }
}