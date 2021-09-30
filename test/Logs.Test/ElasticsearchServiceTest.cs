using LogsVaivoa.Interface;
using LogsVaivoa.Models;
using LogsVaivoa.Services;
using Moq;
using Moq.AutoMock;
using Nest;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
namespace Logs.Test
{
    public class ElasticsearchServiceTest
    {
        private Mock<IElasticsearchService> _elkMock;
        private Mock<IElasticClient> _elkClient;
        private AutoMocker _mock;

        public ElasticsearchServiceTest()
        {
            _mock = new AutoMocker();
            _elkMock = _mock.GetMock<IElasticsearchService>();
            _elkClient = _mock.GetMock<IElasticClient>();
        }
    }
}
