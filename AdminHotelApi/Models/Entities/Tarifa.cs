using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AdminHotelApi.Models
{
    [Table("Tarifas")]
    public class Tarifa : Entity
    {
        [Key, Column(Order = 0)]
        public int HotelId { get; set; }
        [Key, Column(Order = 1)]
        public int TipoHabitacionId { get; set; }
        [Key, Column(Order = 2)]
        public int TarifaId { get; set; }
        public int DiaSemanaId { get; set; }
        public int Personas { get; set; }
        public double Precio { get; set; }
        public virtual TipoHabitacion TipoHabitacion { get; set; }
    }
}