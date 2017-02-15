using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarProject.DatabaseProxy.Extensions
{
    using System.Net.Http;
    using System.Security.Claims;

    public static class HttpRequestMessageExtensions
    {
        public static string GetUserName(this HttpRequestMessage message)
        {
            return message.GetOwinContext().
                  Authentication.User.Claims.
                  First(x => x.Type.Equals(ClaimTypes.Name)).Value;

        }
    }
}
