﻿using System.Threading.Tasks;
using LogsVaivoa.Models;

namespace LogsVaivoa.Interface
{
    public interface IElasticsearchService
    {
        public Task<bool> SendToElastic<T>(T log, string index) where T : class;
    }
}