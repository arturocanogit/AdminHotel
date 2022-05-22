using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Http.Filters;

namespace AdminHotelApi.Models
{
    public class CustomExceptionFilter: ExceptionFilterAttribute
    {
        private static ILog Log;

        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnException(actionExecutedContext);

            string exceptionMessage;
            MethodBase method;
            if (actionExecutedContext.Exception.InnerException == null)
            {
                exceptionMessage = actionExecutedContext.Exception.ToString();
                method = actionExecutedContext.Exception.TargetSite;
            }
            else
            {
                exceptionMessage = actionExecutedContext.Exception.InnerException.ToString();
                method = actionExecutedContext.Exception.InnerException.TargetSite;
            }
            Log = LogManager.GetLogger(method.DeclaringType);
            Log.Error(exceptionMessage);

            //We can log this exception message to the file or database.
            var response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent("Internal Server Error.Please Contact your Administrator.")
            };
            actionExecutedContext.Response = response;
        }
    }
}