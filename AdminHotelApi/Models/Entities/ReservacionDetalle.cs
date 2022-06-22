using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AdminHotelApi.Models.Entities
{
    public class ReservacionDetalle : Entity
    {
        [Key, Column(Order = 0)]
        public int HotelId { get; set; }
        [Key, Column(Order = 1)]
        public int ReservacionId { get; set; }
        [Key, Column(Order = 2)]
        public int ReservacionDetalleId { get; set; }
        public int? HabitacionId { get; set; }
        public int TipoHabitacionId { get; set; }
        public int Personas { get; set; }
        public int Precio { get; set; }
        public bool Desayuno { get; set; }
        [JsonIgnore]
        public virtual Reservacion Reservacion { get; set; }
        [JsonIgnore]
        public virtual Habitacion Habitacion { get; set; }
        [JsonIgnore]
        public virtual TipoHabitacion TipoHabitacion { get; set; }
       
    }
}