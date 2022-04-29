using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AdminHotelApi.Models
{
    [Table("TarifasFestivos")]
    public class TarifaFestivo : Entity
    {
        [Key, Column(Order = 0)]
        public int HotelId { get; set; }
        [Key, Column(Order = 1)]
        public int TarifaId { get; set; }
        public int TipoHabitacionId { get; set; }
        public double Precio { get; set; }
        public double PrecioExtra { get; set; }
        public virtual TipoHabitacion TipoHabitacion { get; set; }
    }
}