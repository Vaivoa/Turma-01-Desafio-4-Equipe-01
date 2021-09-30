using System;
using System.Collections.Generic;
using System.Linq;
using Nest;

namespace LogsVaivoa.Models
{
    public class ApplicationInsightResponse
    {
        public class Column
        {
            public string name { get; set; }
            public string type { get; set; }

            public Column() { }

            public Column(string name, string type)
            {
                this.name = name;
                this.type = type;
            }
        }

        public class Table
        {
            public string name { get; set; }
            public List<Column> columns { get; set; }
            public List<List<string>> rows { get; set; }

            public Table()
            {
                
            }
            public Table(string name, List<Column> columns, List<List<string>> rows)
            {
                this.name = name;
                this.columns = columns;
                this.rows = rows;
            }
        }

       
        public List<Table> tables { get; set; }

        public ApplicationInsightResponse()
        {
            tables = new List<Table>();
        }
        
        public List<LogApplicationInsight> MapToLog()
        {
            try
            {
                return tables[0].rows.Select(i =>
                    new LogApplicationInsightBuild().Timestamps(i[0])
                                                    .Id(i[1])
                                                    .OperationName(i[2])
                                                    .Success(i[3])
                                                    .StatusCode(i[4])
                                                    .Duration(i[5])
                                                    .ClientCity(i[6])
                                                    .ClientCountry(i[7])
                                                    .ClientIP(i[8])
                                                    .ClientType(i[9]).Build()
               ).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}