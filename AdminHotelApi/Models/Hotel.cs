using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AdminHotelApi.Models
{
    [Table("Hoteles")]
    public class Hotel : Entity
    {
        [Key]
        public int HotelId { get; set; }
        [Required, MaxLength(128)]
        public string Nombre { get; set; }
    }
}