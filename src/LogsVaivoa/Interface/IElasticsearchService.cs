using System.Threading.Tasks;
using LogsVaivoa.Models;

namespace LogsVaivoa.Interface
{
    public interface IElasticsearchService
    {
        public Task<bool> SendToElastic(LogApplicationInsight log, string index);
        public Task<bool> SendToElastic(Log log, string index);
    }
}