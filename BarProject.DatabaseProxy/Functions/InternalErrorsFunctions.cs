using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarProject.DatabaseProxy.Functions
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.ExceptionHandling;
    using DatabaseConnector;
    using Extensions;
    using Models.Utilities;

    public static class InternalErrorsFunctions
    {
        public static void LogException(ExceptionData data, ExceptionLoggerContext context)
        {
            var userName = context.Request.GetUserName();
            var builder = new StringBuilder();

            builder.Append("Calling user: ");
            builder.Append(userName != null ? $"{userName} " : "Anonymous ");

            builder.Append("Method arguments : ");
            foreach (var item in
                ((ApiController)context.ExceptionContext.ControllerContext.Controller).ActionContext.ActionArguments)
            {
                var objects = Utilities.Serialize(item.Value);
                builder.Append($"{item.Key} : {objects}\n");
            }
            builder.AppendLine();
            if (data.Arguments != null)
            {
                builder.Append("Focus :");
                foreach (var arg in data.Arguments)
                {
                    builder.Append(Utilities.Serialize(arg));
                }
            }
            builder.AppendLine();

            builder.Append(";");
            var str = ((ApiController)context.ExceptionContext.ControllerContext.Controller).ActionContext.ActionDescriptor.ActionName;
            LogException(data.Code.ToString(), data.Time, data.Reason, "Method: " + str, builder.ToString(), data.InnerMessage);
        }

        public static void LogException(string errorName, DateTime errorTime, string message, string stackTrace = "", string context = "", string innerMessage = "")
        {
            try
            {
                using (var db = new Entities())
                {
                    db.createInternalError(errorName, errorTime, message, stackTrace, context, innerMessage);
                }
            }
            catch
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("An error occurred, please try again or contact the administrator."),
                    ReasonPhrase = "Critical Exception"
                });
            }
        }
    }
}
