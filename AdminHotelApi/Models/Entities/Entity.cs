using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminHotelApi.Models
{
    public class Entity
    {
        public Entity()
        {
            Activo = true;
            FechaAlta = DateTime.Now;
        }
        public bool Activo { get; set; }
        public DateTime FechaAlta { get; set; }
        public DateTime? FechaUpdate { get; set; }
    }
}