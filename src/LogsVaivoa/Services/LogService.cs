using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using Dapper.Contrib.Extensions;

namespace LogsVaivoa.Services
{
    public static class LogService
    {
        private static SqlConnection DbConnection()
        {
            return new SqlConnection(Environment.GetEnvironmentVariable("SqlConnection"));
        }

        public static (bool, object) InsertLog(LogModel log)
        {
            var errors = log.GetErrors();

            if (errors.Any()) return (false, errors);

            using var db = DbConnection();
            db.Insert(log);

            return (true, log);
        }
        
        
        
    }
}