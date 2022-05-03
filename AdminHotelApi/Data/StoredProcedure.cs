using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace AdminHotelApi.Data
{
    internal class StoredProcedure
    {
        public static AdminHotelApiContext Db { get; set; }

        public static IEnumerable<T> Execute<T>(string spName)
        {
            DbRawSqlQuery<T> result = Db.Database.SqlQuery<T>(spName);
            return result.ToList();
        }
    }
}