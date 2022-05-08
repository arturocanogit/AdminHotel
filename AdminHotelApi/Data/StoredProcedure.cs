using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace AdminHotelApi.Data
{
    internal class StoredProcedure
    {
        public static AdminHotelApiContext Db { get; set; }

        public static IEnumerable<T> Execute<T>(string spName, object[] parameters)
        {
            DbRawSqlQuery<T> result = Db.Database.SqlQuery<T>(spName, parameters);
            return result.ToList();
        }
    }
}