using Moq;
using System;
using Xunit;
using LogsVaivoa.Interface;
using Microsoft.Extensions.Logging;
using LogsVaivoa.Services;
using Moq.AutoMock;
using LogsVaivoa.Models;

namespace Logs.Test
{
    public class LogsServiceTest
    {
        private Mock<IElasticsearchService> _elkMock;
        private Mock<ILogger<LogService>> _logServiceMock;
        private Mock<ILogRepository> _logRepositoryMock;
        private AutoMocker _mock;

        public LogsServiceTest()
        {
            _mock = new AutoMocker();
            _elkMock = _mock.GetMock<IElasticsearchService>();
            _logServiceMock = _mock.GetMock<ILogger<LogService>>();
            _logRepositoryMock = _mock.GetMock<ILogRepository>();
        }


        [Trait("PostLog","PostLog_False")]
        [Fact(DisplayName = "postlog_deveRetornarFalseEListaDeErro_quandoEntidadeLogForInvalida")]
        public async void postlog_deveRetornarFalseEListaDeErro_quandoEntidadeLogForInvalida()
        {
            var log = new Log("Aécio", null, "Aécio");

            var sut = _mock.CreateInstance<LogService>();

            //Act
            var result = await sut.PostLog(log);

            //Assert
            Assert.False(result.Item1);
        }

        [Trait("PostLog", "PostLog_True")]
        [Fact(DisplayName = "postlog_deveRetornarTrueELog_quandoOperaçaoForValida")]
        public async void postlog_deveRetornarTrueELog_quandoOperaçaoForValida()
        {
            var log = new Log("Aécio", "Aécio", "Aécio");

            var sut = _mock.CreateInstance<LogService>();

            //Act
            var result = await sut.PostLog(log);

            //Assert
            Assert.True(result.Item1);
            Assert.Equal(log,result.Item2);
            _elkMock.Verify(e => e.SendToElastic(It.IsAny<Log>(), It.IsAny<string>()), Times.Once);
            _logRepositoryMock.Verify(l => l.InsertLogDb(It.IsAny<Log>()), Times.Once);
        }
    }
}
