using System;
using System.Data.SqlClient;
using LogsVaivoa.Interface;

namespace LogsVaivoa.Services
{
    class DbContext : IDbContext
    {
        private readonly string _dbconnection;
        private SqlConnection _sqlConnection; 

        public DbContext()
        {
            _dbconnection = Environment.GetEnvironmentVariable("SqlConnection");
        }
        public SqlConnection GetDbConnection()
        {
            return _sqlConnection ??= new SqlConnection(_dbconnection);
        }
    }
}