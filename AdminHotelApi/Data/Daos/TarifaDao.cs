using AdminHotelApi.Models;
using AdminHotelApi.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminHotelApi.Data
{
    public class TarifaDao
    {
        public static AdminHotelApiContext Db { get; set; }

        public static IEnumerable<TarifaDto> GetTarifas(int tipoHabitacionId, DateTime fechaEntrada, DateTime fechaSalida)
        {
            StoredProcedure.Db = Db;
            IEnumerable<TarifaDto> tarifas =
                StoredProcedure.Execute<TarifaDto>("sp_get_tarifas {0}, {1}, {2}", new object[]
                {
                    tipoHabitacionId,
                    fechaEntrada,
                    fechaSalida
                });
            return tarifas;
        }
    }
}
