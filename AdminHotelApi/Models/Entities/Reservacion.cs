using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AdminHotelApi.Models.Entities
{
    [Table("Reservaciones")]
    public class Reservacion : Entity
    {
        [Key, Column(Order = 0)]
        public int HotelId { get; set; }
        [Key, Column(Order = 1)]
        public int ReservacionId { get; set; }
        public int ClienteId { get; set; }
        [Required, MaxLength(64)]
        public string Folio { get; set; }
        public DateTime FechaEntrada { get; set; }
        public DateTime FechaSalida { get; set; }
        public virtual Cliente Cliente { get; set; }
    }
}