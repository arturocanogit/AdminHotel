using AdminHotelApi.Controllers.Api;
using AdminHotelApi.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace AdminHotelApi.Tests
{
    [TestClass]
    public class RservacionesTest
    {
        [TestMethod]
        public void GetDisponibilidadesTest()
        {
            IHttpActionResult result = new ReservacionesController()
                .GetDisponibilidades(new RequestDisponibilidad
            {
                FechaInicio = DateTime.Now,
                FechaFin = DateTime.Now.AddDays(5),
                Adultos = 5,
                Menores = 2
            });

            Assert.IsTrue(result != null);
        }
    }
}
