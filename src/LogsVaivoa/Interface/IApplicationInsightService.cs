using System.Threading.Tasks;

namespace LogsVaivoa.Interface
{
    public interface IApplicationInsightService
    {
        public Task SendMetricToElastic();
    }
}