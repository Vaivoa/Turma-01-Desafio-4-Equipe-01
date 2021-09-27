using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using LogsVaivoa;
using LogsVaivoa.Models;
using LogsVaivoa.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

[assembly: WebJobsStartup(typeof(Startup))]
namespace LogsVaivoa
{
    

    public class LogsFunction
    {
        private readonly LogService _logService;
        private readonly ApplicationInsightService _appService;
        public LogsFunction(LogService logService, ApplicationInsightService appService, ILogger<LogsFunction> logger)
        {
            _logService = logService;
            _appService = appService;
        }

        [FunctionName("LogsFunction")]
        [OpenApiOperation(operationId: "Run")]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(Log), Required = true)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Created, contentType: "application/json", bodyType: typeof(Log), Description = "Created")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(List<ValidationFailure>), Description = "Fail Validation")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous,"post", Route = null)] HttpRequest req)
        {
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<Log>(requestBody);

            await _appService.SendMetricToElastic();
            var (status, result) = await _logService.PostLog(data);

            if (status) return new CreatedResult("", result);

            return new BadRequestObjectResult(result);
        }
    }

    

}

