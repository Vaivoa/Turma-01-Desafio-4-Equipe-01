using System.Collections.Generic;
using LogsVaivoa.Models;
using LogsVaivoa.Services;
using Xunit;

namespace Logs.Test
{
    public class ApplicationInsightResponseTest
    {
        [Fact]
        public void ApplicationInsightResponse_deveRealizarAConversaoDoObjeto()
        {
            //Arrange
            var rows = new List<List<string>> { new List<string>() };
            var columns = new List<ApplicationInsightResponse.Column>(); 
            for (var i = 0; i < 10; i++) rows[0].Add(i==5 ? "2,00":"any");
            columns.Add(new ApplicationInsightResponse.Column("any", "any"));
            
            var table = new ApplicationInsightResponse.Table("any", columns, rows);
            var response = new ApplicationInsightResponse();
            response.tables.Add(table);
            
            //Act
            var result = response.MapToLog();

            //Assert
            Assert.NotNull(result);
        }
        
        [Fact]
        public void ApplicationInsightResponse_deveRetornarNullQuandoConversaoForInvalida()
        {
            //Arrange
            var response = new ApplicationInsightResponse();
            
            //Act
            var result = response.MapToLog();

            //Assert
            Assert.Null(result);
        }
    }
}