namespace BarProject.DatabaseProxy.Models.ExceptionHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Core;
    using System.Data.SqlClient;
    using System.Net;
    using System.Text.RegularExpressions;
    using Utilities;

    /// <summary>
    /// provides exception to being sent via http
    /// </summary>
    public class ResponseException : Exception
    {
        public ExceptionData ExceptionData { get; private set; }

        public ResponseException(HttpStatusCode code, string reason, string innerMessage = "", Utilities.ExceptionType type = Utilities.ExceptionType.Unknown,
            params object[] arguments)
        {
            ExceptionData = new ExceptionData
            {
                Reason = reason,
                InnerMessage = innerMessage,
                Type = type,
                Code = code,
                Arguments = arguments,
                Time = Utilities.GetServerTime()
            };
        }

        public ResponseException(HttpStatusCode e, string reason, Utilities.ExceptionType type = Utilities.ExceptionType.Unknown, params object[] arguments) :
            this(e, reason, "", type, arguments)
        {

        }

        private static Dictionary<int, HttpStatusCode> CodeForDatabaseExceptionsDictionary { get; set; }
        private static Dictionary<int, Regex> ReasonForDatabaseExceptionDictionary { get; set; }

        static ResponseException()
        {
            CodeForDatabaseExceptionsDictionary = new Dictionary<int, HttpStatusCode>
            {
                {60000, HttpStatusCode.NotFound},
                {60001, HttpStatusCode.Conflict},
                {13583, HttpStatusCode.Conflict},
                {60002, HttpStatusCode.RequestedRangeNotSatisfiable},
                {60003, HttpStatusCode.PreconditionFailed},
                {60004, HttpStatusCode.Unauthorized},
                {60005, HttpStatusCode.Forbidden},
                {60007, HttpStatusCode.ExpectationFailed},
            };
            ReasonForDatabaseExceptionDictionary = new Dictionary<int, Regex>
            {
                {2627, uniqueKeyViolation},
            };
        }

        public ResponseException(Exception e, params object[] arguments)
        : this(HttpStatusCode.InternalServerError, e.Message, e.InnerException?.Message, Utilities.ExceptionType.Unknown, arguments)
        {
            CheckAllExceptions(e, arguments);

        }
        private static Regex uniqueKeyViolation = new Regex("Violation of (?:.*?) constraint (?:.*?). Cannot insert duplicate key in object (?:.*?). The duplicate key value is \\((.*?)\\)");
        private void SqlExceptionDataFill(object[] arguments, SqlException sql)
        {
            var exceptionSql = sql;
            ExceptionData.Code = CodeForDatabaseExceptionsDictionary.ContainsKey(exceptionSql.Number)
                ? CodeForDatabaseExceptionsDictionary[exceptionSql.Number]
                : HttpStatusCode.InternalServerError;
            if (ReasonForDatabaseExceptionDictionary.ContainsKey(exceptionSql.Number))
            {
                var reg = ReasonForDatabaseExceptionDictionary[exceptionSql.Number];
                var match = reg.Match(exceptionSql.Message);
                ExceptionData.Reason = $"Value '{match.Groups[1].Value}' has to be unique but already exists in database";
            }
            else
                ExceptionData.Reason = exceptionSql.Message;
            ExceptionData.Arguments = arguments;
            ExceptionData.Type = Utilities.ExceptionType.Database;
            ExceptionData.Time = Utilities.GetServerTime();
        }

        public ResponseException(Exception e, HttpStatusCode code, params object[] arguments) :
                this(code, e.Message, e.InnerException?.Message, Utilities.ExceptionType.Unknown, arguments)
        {
            CheckAllExceptions(e, arguments);
        }
        public ResponseException(Exception e, Utilities.ExceptionType type, params object[] arguments) : this(HttpStatusCode.InternalServerError, e.Message,
                e.InnerException?.Message, type, arguments)
        {
            CheckAllExceptions(e, arguments);
        }
        public ResponseException(Exception e, HttpStatusCode code, Utilities.ExceptionType type, params object[] arguments) :
            this(HttpStatusCode.InternalServerError, e.Message, e.InnerException?.Message, type, arguments)
        {
            CheckAllExceptions(e, arguments);
        }
        private void CheckAllExceptions(Exception e, object[] args)
        {
            ChangeDataIfSqlException(e, args);
            CheckDataIfArgumentException(e);
            IsResponseException(e);

        }

        private void IsResponseException(Exception exception)
        {
            var exc = exception as ResponseException;
            if (exc != null)
            {
                ExceptionData = exc.ExceptionData;
            }

        }

        private void ChangeDataIfSqlException(Exception e, object[] arguments)
        {
            if (e is EntityCommandExecutionException)
            {
                e = e.InnerException;
            }
            var sql = e as SqlException;
            if (sql != null)
            {
                SqlExceptionDataFill(arguments, sql);
            }
            else
            {
                ExceptionData.Exception = e;
            }
        }

        private void CheckDataIfArgumentException(Exception e)
        {
            if (e is ArgumentException)
            {
                ExceptionData.Code = HttpStatusCode.BadRequest;
                ExceptionData.Reason = e.Message;
            }
            ExceptionData.Exception = e;

        }
    }
}
