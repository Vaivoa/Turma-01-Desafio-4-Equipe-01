namespace LogsVaivoa.Models
{
    public class LogApplicationInsight
    {
        public LogApplicationInsight(string timestamps, string id, string operationName, string success, string statusCode, string duration, string clientCity, string clientCountry, string clientIp, string clientType)
        {
            Timestamps = timestamps;
            Id = id;
            OperationName = operationName;
            Success = success;
            StatusCode = statusCode;
            Duration = duration;
            ClientCity = clientCity;
            ClientCountry = clientCountry;
            ClientIP = clientIp;
            ClientType = clientType;
        }

        public string Timestamps { get; set; }
        public string Id { get; set; }
        public string OperationName { get; set; }
        public string Success { get; set; }
        public string StatusCode { get; set; }
        public string Duration { get; set; }

        public string ClientCity { get; set; }
        public string ClientCountry { get; set; }
        public string ClientIP { get; set; }

        public string ClientType { get; set; }
    }
}