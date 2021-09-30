using System;

namespace LogsVaivoa.Models
{
    public class LogApplicationInsight
    {

        public string Timestamps { get; set; }
        public string Id { get; set; }
        public string OperationName { get; set; }
        public string Success { get; set; }
        public string StatusCode { get; set; }
        public double Duration { get; set; }

        public string ClientCity { get; set; }
        public string ClientCountry { get; set; }
        public string ClientIP { get; set; }

        public string ClientType { get; set; }
    }

    public class LogApplicationInsightBuild
    {
        private readonly LogApplicationInsight _log = new LogApplicationInsight();

        public LogApplicationInsightBuild Timestamps(string value)
        {
            _log.Timestamps = value;
            return this;
        } 
        public LogApplicationInsightBuild Id(string value)
        {
            _log.Id = value;
            return this;
        } 
        public LogApplicationInsightBuild OperationName(string value)
        {
            _log.OperationName = value;
            return this;
        } 
        public LogApplicationInsightBuild StatusCode(string value)
        {
            _log.StatusCode = value;
            return this;
        } 
        public LogApplicationInsightBuild Success(string value)
        {
            _log.Success = value;
            return this;
        } 
        public LogApplicationInsightBuild ClientCity(string value)
        {
            _log.ClientCity = value;
            return this;
        } 
        public LogApplicationInsightBuild ClientCountry(string value)
        {
            _log.ClientCountry = value;
            return this;
        }
        public LogApplicationInsightBuild ClientIP(string value)
        {
            _log.ClientIP = value;
            return this;
        } 
        public LogApplicationInsightBuild ClientType(string value)
        {
            _log.ClientType = value;
            return this;
        } 
        public LogApplicationInsightBuild Duration(string value)
        {
            _log.Duration = (int)double.Parse(value);
            return this;
        }

        public LogApplicationInsight Build() => _log;
    }
}