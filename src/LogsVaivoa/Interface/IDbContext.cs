using System.Data.SqlClient;

namespace LogsVaivoa.Interface
{
    public interface IDbContext
    {
        public SqlConnection GetDbConnection();
    }
}