namespace BarProject.DatabaseProxy.Models.Utilities
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Security.Claims;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web.Http.ModelBinding;
    using ExceptionHandlers;
    using Microsoft.Owin;
    using Newtonsoft.Json;

    public static class Utilities
    {
        public class RegexUtilities
        {
            bool invalid = false;

            public bool IsValidEmail(string strIn)
            {
                invalid = false;
                if (string.IsNullOrEmpty(strIn))
                    return false;

                // Use IdnMapping class to convert Unicode domain names.
                try
                {
                    strIn = Regex.Replace(strIn, @"(@)(.+)$", this.DomainMapper,
                                          RegexOptions.None, TimeSpan.FromMilliseconds(200));
                }
                catch (RegexMatchTimeoutException)
                {
                    return false;
                }

                if (invalid)
                    return false;

                // Return true if strIn is in valid e-mail format.
                try
                {
                    return Regex.IsMatch(strIn,
                          @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                          @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                          RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
                }
                catch (RegexMatchTimeoutException)
                {
                    return false;
                }
            }

            private string DomainMapper(Match match)
            {
                // IdnMapping class with default property values.
                IdnMapping idn = new IdnMapping();

                string domainName = match.Groups[2].Value;
                try
                {
                    domainName = idn.GetAscii(domainName);
                }
                catch (ArgumentException)
                {
                    invalid = true;
                }
                return match.Groups[1].Value + domainName;
            }
        }

        public static DateTime GetServerTime()
        {
            return DateTime.Now;
        }

        public static byte[] GeneratePassword(byte[] hash, byte[] salt)
        {
            return hash;
        }

        public static byte[] GenerateSalt()
        {
            return new byte[0];
        }

        internal static string GetUser(IOwinContext context)
        {
            return context.Authentication.User.Claims.First(x => x.Type.Equals(ClaimTypes.Email)).Value;
        }
        internal static string Serialize(object o)
        {
            if (o == null)
                return "null";
            var ser = new JsonSerializer();
            var builder = new StringBuilder();
            using (var stream = new StringWriter(builder))
            {
                ser.Serialize(stream, o);
                var re = stream.ToString();
                return re;
            }

        }

        /// <summary>
        /// Throws BadRequest (400) if model is invalid
        /// </summary>
        /// <param name="ms"></param>
        public static void CheckModelState(ModelStateDictionary ms)
        {
            if (!ms.IsValid)
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("Model is invalid: ");
                foreach (var value in ms.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        builder.Append(string.IsNullOrEmpty(error.ErrorMessage) ? "" : error.ErrorMessage + " ");
                        builder.Append(error.Exception?.Message ?? "");
                        builder.AppendLine();
                    }
                }
                throw new ResponseException(HttpStatusCode.BadRequest, builder.ToString(), ExceptionType.User, ms);
            }
        }
        public enum ExceptionType { Database, Server, Unknown, User }

    }
}
