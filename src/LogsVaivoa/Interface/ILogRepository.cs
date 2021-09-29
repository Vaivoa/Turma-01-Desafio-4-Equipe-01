using System.Threading.Tasks;
using LogsVaivoa.Models;

namespace LogsVaivoa.Interface
{
    public interface ILogRepository
    {
        Task<bool> InsertLogDb(Log log);
    }
}