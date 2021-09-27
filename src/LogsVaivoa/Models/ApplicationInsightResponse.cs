using System;
using System.Collections.Generic;
using System.Linq;
using LogsVaivoa.Services;

namespace LogsVaivoa.Models
{
    public class ApplicationInsightResponse
    {
        public class Table
        {
            public string name { get; set; }
            public List<List<string>> rows { get; set; }
        }
        private List<Table> Tables { get; set; }


        public LogApplicationInsight MapToLog()
        {
            try
            {
                return Tables[0].rows.Select(i =>
                    new LogApplicationInsight(
                        i[0], i[1], i[2], i[3], i[4], i[5], 
                        i[6], i[7], i[8], i[9])
                ).First();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}