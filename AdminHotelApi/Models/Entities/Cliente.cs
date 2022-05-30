using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AdminHotelApi.Models
{
    [Table("Clientes")]
    public class Cliente : Entity
    {
        [Key, Column(Order = 0)]
        public int HotelId { get; set; }
        [Key, Column(Order = 1)]
        public int ClienteId { get; set; }
        [Required, MaxLength(128)]
        public string Nombre { get; set; }
        [MaxLength(16)]
        public string Telefono { get; set; }
        [MaxLength(32)]
        public string Email { get; set; }
        [JsonIgnore]
        public virtual Hotel Hotel { get; set; }
    }
}