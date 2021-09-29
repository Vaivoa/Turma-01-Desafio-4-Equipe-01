using System.Data;
using System.Data.SqlClient;

namespace LogsVaivoa.Interface
{
    public interface IDbContext
    {
        public IDbConnection GetDbConnection();
    }
}