using System;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Dapper.Contrib.Extensions;
using LogsVaivoa.Interface;
using LogsVaivoa.Models;
using LogsVaivoa.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.AutoMock;
using Moq.Dapper;
using Nest;
using Newtonsoft.Json;
using ServiceStack.OrmLite;
using Xunit;

namespace Logs.Test
{
    public class LogServiceTest
    {
        private readonly AutoMocker _mock;
        private readonly Mock<IElasticsearchService> _elasticService;
        private readonly Mock<ILogger<LogService>> _logger;
        private readonly Mock<IDbContext> _dbContext;

        public LogServiceTest()
        {
            _mock = new AutoMocker();
            _elasticService = _mock.GetMock<IElasticsearchService>();
            _logger = _mock.GetMock<ILogger<LogService>>();
            _dbContext = _mock.GetMock<IDbContext>();
        }

        [Fact]
        public void deveRetornarFalseEOsErros_QuandoLogForInvalido()
        {
            //Arrange

            //Act
            
            //Assert
        }
        
        [Fact]
        public async void deveRetornarTrue_QuandoLogForValido()
        {
            //Arrange
            /*var sqlMock = new Mock<IDbConnection>();
            sqlMock.SetupDapperAsync(d =>
                d.InsertAsync(It.IsAny<Log>(), It.IsAny<IDbTransaction>(), It.IsAny<int?>(), It.IsAny<ISqlAdapter>()));
            _dbContext.Setup(d => 
                d.GetDbConnection())
                .Returns(sqlMock.Object);*/
           // var log = new Log("any", "any", null);
            //var sut = _mock.CreateInstance<LogService>();
            
            //Act
            //var result = await sut.PostLog(log);

            //Assert
            //Assert.True(result.Item1);
            //_elasticService.Verify(e => e.SendToElastic(It.IsAny<Log>(), "anyIndex"), Times.Once);
            //sqlMock.Verify(s => s.InsertAsync(It.IsAny<Log>(), null, null, null), Times.Once);
        }
    }
}