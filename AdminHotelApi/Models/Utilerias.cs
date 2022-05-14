using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace AdminHotelApi.Models
{
    public class Utilerias
    {
        public static TResult Mapeador<TResult, TSource>(TSource tipoHabitacionDto) where TResult : new()
        {
            TResult result = Activator.CreateInstance<TResult>();
            Type typeSource = typeof(TSource);
            foreach (var property in result.GetType().GetProperties())
            {
                var propertySource = typeSource.GetProperty(property.Name);
                if (propertySource != null)
                {
                    property.SetValue(result,
                    typeSource.GetProperty(property.Name).GetValue(tipoHabitacionDto));
                }


            }
            return result;
        }

        public static byte[] StreamToByteArray(Stream input)
        {
            MemoryStream ms = new MemoryStream();
            input.CopyTo(ms);
            return ms.ToArray();
        }
    }
}
