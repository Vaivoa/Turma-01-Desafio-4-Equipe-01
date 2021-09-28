using System.Threading.Tasks;
using LogsVaivoa.Models;

namespace LogsVaivoa.Interface
{
    public interface ILogService
    {
        public Task<(bool, object)> PostLog(Log log);
    }
}